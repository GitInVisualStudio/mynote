<?php
require_once("Utils/DBConnection.php");
require_once("Utils/Globals.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/WrongFormatException.php");
require_once("Exceptions/WrongCredentialsException.php");
require_once("Exceptions/AuthTokenExpiredException.php");
require_once("Exceptions/InvalidHigherIDException.php");
require_once("Exceptions/ServerException.php");
require_once("Exceptions/InsufficientPermissionsException.php");

set_error_handler(function($errno, $errstr, $errfile, $errline, $errcontext) {
    // error was suppressed with the @-operator
    if (0 === error_reporting()) {
        return false;
    }

    throw new Exception($errstr, $errno);
});

function main(array $post) : array {
    $connection = new DBConnection("127.0.0.1", "root", "", "mynote");

    if (!(array_key_exists("auth", $post) && array_key_exists("course", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $course = $post["course"];

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $course_id = $connection->realEscapeString($course["onlineID"]);
    $semester_id = $connection->realEscapeString($course["semesterOnlineID"]);
    $course_name = $connection->realEscapeString($course["name"]);
    $course_color = $connection->realEscapeString($course["color"]);
    $course_created = $connection->realEscapeString($course["created"]);
    $course_created = Globals::cSharpDateTimeToPHPDateTime($course_created);
    $course["created"] = $course_created;
    $icon = $course["icon"];
    $icon_id = $icon["id"];
    $course["icon"] = $icon_id;

    $result = $connection->select("semester", ["id"], "`id` = '{$semester_id}'");
    if (sizeof($result) == 0)
        throw new InvalidHigherIDException(); // given semester was not valid
    else if (sizeof($result) > 1)
        throw new ServerException(); // two semesters with same id existed (shouldn't happen)

    $result = $connection->select("icon", ["id"], "`id` = '{$icon_id}'");
    if (sizeof($result) == 0)
        throw new InvalidHigherIDException(); // given icon was not valid
    else if (sizeof($result) > 1)
        throw new ServerException(); // two semesters with same id existed (shouldn't happen)

    $result = $connection->select("course", ["user_id"], "`id` = '{$course_id}'");
    if (sizeof($result) > 0) {
        $result = $result[0];
        $existing_user_id = $result["user_id"];
        if ($existing_user_id != $user_id)
            throw new InsufficientPermissionsException(); // user tried to update course that wasn't theirs
        $connection->query("UPDATE `course` SET `name` = '{$course_name}', `created` = '{$course_created}', `semester_id` = '{$semester_id}', `color` = '{$course_color}', `icon_id` = '{$icon_id}' WHERE `id` = {$course_id}");
        $result = ["result" => "ok", "courseID" => $course_id];
    } else {
        $rows = [];
        $rows[] = ["user_id" => $user_id, "semester_id" => $semester_id, "name" => $course_name, "created" => $course_created, "color" => $course_color, "icon_id" => $icon_id];
        $ids = $connection->insert("course", $rows);
        if (sizeof($ids) == 0)
            throw new ServerException(); // insert query failed
        $course_id = $ids[0];

        $rows = [];
        $rows[] = ["user_id" => $user_id, "course_id" => $course_id, "admin" => true];
        $connection->insert("user_in_course", $rows);

        $result = ["result" => "ok", "courseID" => $course_id];
    }

    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
