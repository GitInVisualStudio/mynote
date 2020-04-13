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
require_once("Exceptions/ObjectDoesntExistException.php");
require_once("Exceptions/IllegalOperationException.php");

set_error_handler(function ($errno, $errstr, $errfile, $errline, $errcontext) {
    // error was suppressed with the @-operator
    if (0 === error_reporting()) {
        return false;
    }

    throw new Exception($errstr, $errno);
});

function main(array $post): array
{
    $connection = Globals::GetDBConnection();

    if (!(array_key_exists("auth", $post) && array_key_exists("courseID", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $course_id = $connection->realEscapeString($post["courseID"]);

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $result = $connection->select("user_in_course", ["interpretation_id"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new InsufficientPermissionsException();
    $result = $result[0];
    $interpretation_id = $result["interpretation_id"];
    $result = $connection->select("course", ["name", "created", "color", "icon_id", "semester_id"], "`id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new ServerException(); // course doesn't exist even though user is in course (shouldn't happen, fk constraints forbid this)

    $result = $result[0];
    $course_name = $result["name"];
    $course_created = $result["created"];
    $course_color = $result["color"];
    $icon_id = $result["icon_id"];
    $semester_id = $result["semester_id"];

    if ($interpretation_id != null) {
        $result = $connection->select("interpretation", [], "`id` = '{$interpretation_id}'");
        $result = $result[0];
        if ($result["name"] != null)
            $course_name = $result["name"];
        if ($result["color"] != null)
            $course_color = $result["color"];
        if ($result["icon_id"] != null)
            $icon_id = $result["icon_id"];
    }

    $course = ["name" => $course_name, "color" => $course_color, "iconID" => $icon_id, "onlineID" => $course_id, "semesterOnlineID" => $semester_id, "created" => $course_created, "testLocalIDs" => []];
    $array = ["result" => "ok", "course" => $course];
    return $array;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
