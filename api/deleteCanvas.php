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
    $connection = new DBConnection("127.0.0.1", "root", "", "mynote");

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

    $result = $connection->select("canvas", ["user_id", "path"], "`id` = '{$canvas_id}'");
    if (sizeof($result) === 0)
        throw new ObjectDoesntExistException();

    $result = $result[0];
    $existing_user_id = $result["user_id"];
    $path = $result["path"];
    if ($user_id != $existing_user_id)
        throw new InsufficientPermissionsException();

    $connection->query("DELETE FROM `canvas` WHERE `id` = '{$canvas_id}'");
    unlink($path);

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
