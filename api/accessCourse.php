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

    if (!(array_key_exists("auth", $post) && array_key_exists("courseID", $post) && array_key_exists("password", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $course_id = $connection->realEscapeString($post["courseID"]);
    $password = $connection->realEscapeString($post["password"]);

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $result = $connection->select("user_in_course", ["user_id"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}'");
    if (sizeof($result) > 0)
        return ["result" => "ok"];

    $result = $connection->select("course", ["salt"], "`id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new ObjectDoesntExistException(); // course doesn't exist
    $result = $result[0];
    $salt = $result["salt"];
    $password = Globals::hashAndSalt($password, $salt);

    $result = $connection->select("course", ["id"], "`id` = '{$course_id}' AND `password` = '{$password}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // entered password was wrong

    $rows = [];
    $rows[] = ["user_id" => $user_id, "course_id" => $course_id, "member_since" => "NOW()"];
    $connection->insert("user_in_course", $rows);

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
