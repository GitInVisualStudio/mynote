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

    if (!(array_key_exists("auth", $post) && array_key_exists("test", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $test = $post["test"];

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $course_id = $connection->realEscapeString($test["courseOnlineID"]);
    $test_id = $connection->realEscapeString($test["onlineID"]);
    $test_topic = $connection->realEscapeString($test["topic"]);
    $test_date = $connection->realEscapeString($test["date"]);
    $test_date = Globals::cSharpDateTimeToPHPDateTime($test_date);
    $test_type = $connection->realEscapeString($test["type"]);
    $test_type = Globals::intToTestType($test_type);

    $result = $connection->select("course", ["id"], "`id` = '{$course_id}'");
    if (sizeof($result) == 0)
        throw new InvalidHigherIDException(); // given semester was not valid
    else if (sizeof($result) > 1)
        throw new ServerException(); // two semesters with same id existed (shouldn't happen)

    $result = $connection->select("course", [], "`id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new InvalidHigherIDException();

    $result = $connection->select("test", [], "`id` = '{$test_id}'");
    if (sizeof($result) > 0) {
        $result = $connection->select("user_in_course", ["admin"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}'");
        if (sizeof($result) === 0)
            throw new InsufficientPermissionsException(); // user tried to update course that wasn't theirs
        $result = $result[0];
        $admin = $result["admin"] == 1 ? true : false;
        if ($admin)
            $connection->query("UPDATE `test` SET `course_id` = '{$course_id}', `topic` = '{$test_topic}', `date` = '{$test_date}', `type` = '{$test_type}' WHERE `id` = {$test_id}");
        else
            throw new InsufficientPermissionsException();

        $result = ["result" => "ok", "testID" => $test_id];
    } else {
        $rows = [];
        $rows[] = ["user_id" => $user_id, "course_id" => $course_id, "topic" => $test_topic, "date" => $test_date, "type" => $test_type];
        $ids = $connection->insert("test", $rows);
        if (sizeof($ids) == 0)
            throw new ServerException(); // insert query failed
        $test_id = $ids[0];

        $result = ["result" => "ok", "testID" => $test_id];
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
