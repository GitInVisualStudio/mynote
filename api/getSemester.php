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
    $connection = new DBConnection("127.0.0.1", "root", "", "mynote");

    if (!(array_key_exists("auth", $post) && array_key_exists("semesterID", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $semester_id = $connection->realEscapeString($post["semesterID"]);

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $result = $connection->query("SELECT * FROM course c INNER JOIN user_in_course u ON (c.id = u.course_id) WHERE u.user_id = '{$user_id}' AND c.semester_id = '{$semester_id}'");
    if (sizeof($result) === 0)
        throw new InsufficientPermissionsException();

    $result = $connection->select("semester", ["name", "created"], "`id` = '{$semester_id}'");
    if (sizeof($result) === 0)
        throw new ServerException(); // semester doesn't exist even though user is in course in semester (shouldn't happen, fk constraints forbid this)

    $result = $result[0];
    $semester_name = $result["name"];
    $semester_created = $result["created"];

    $semester = ["name" => $semester_name, "created" => $semester_created, "onlineID" => $semester_id];
    $array = ["result" => "ok", "semester" => $semester];
    return $array;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
