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

    if (!(array_key_exists("auth", $post) && array_key_exists("canvasID", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $canvas_id = $connection->realEscapeString($post["canvasID"]);

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $result = $connection->select("canvas", ["course_id", "public", "user_id", "path"], "`id` = '{$canvas_id}'");
    if (sizeof($result) === 0)
        throw new ObjectDoesntExistException(); // requested canvas doesn't exist
    $result = $result[0];
    $course_id = $result["course_id"];
    $public = $result["public"] == 1 ? true : false;
    $maker_id = $result["user_id"];
    $path = $result["path"];

    $result = $connection->select("user_in_course", ["user_id"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new InsufficientPermissionsException(); // user isn't in course of canvas

    if (!($user_id == $maker_id || $public))
        throw new InsufficientPermissionsException();

    $canvas = json_decode(file_get_contents($path));
    $array = ["result" => "ok", "canvas" => $canvas];
    return $array;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
