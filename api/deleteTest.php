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

set_error_handler(function($errno, $errstr, $errfile, $errline, $errcontext) {
    // error was suppressed with the @-operator
    if (0 === error_reporting()) {
        return false;
    }

    throw new Exception($errstr, $errno);
});

function main(array $post) : array {
    $connection = new DBConnection("127.0.0.1", "root", "", "mynote");

    if (!(array_key_exists("auth", $post) && array_key_exists("testID", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $test_id = $post["testID"];

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $test_id = $connection->realEscapeString($test_id);

    $result = $connection->select("test", ["course_id"], "`id` = '{$test_id}'");
    if (sizeof($result) == 0)
        throw new ObjectDoesntExistException(); // given semester was not valid
    else if (sizeof($result) > 1)
        throw new ServerException(); // two semesters with same id existed (shouldn't happen)
    $result = $result[0];
    $course_id = $result["course_id"];
    $result = $connection->select("user_in_course", ["admin"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new InsufficientPermissionsException(); // user tried to update course that wasn't theirs
    $result = $result[0];
    $admin = $result["admin"] == 1 ? true : false;
    if ($admin)
        $connection->query("DELETE FROM `test` WHERE `id` = {$test_id}");
    else
        throw new InsufficientPermissionsException();

    $result = ["result" => "ok"];

    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
