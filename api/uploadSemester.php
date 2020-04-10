<?php
require_once("Utils/DBConnection.php");
require_once("Utils/Globals.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/AuthTokenExpiredException.php");
require_once("Exceptions/WrongCredentialsException.php");
require_once("Exceptions/WrongFormatException.php");
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

    if (!(array_key_exists("auth", $post) && array_key_exists("semester", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $semester = $post["semester"];
    
    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist
    
    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $semester_id = $connection->realEscapeString($semester["onlineID"]);
    $semester_name = $connection->realEscapeString($semester["name"]);
    $semester_created = $connection->realEscapeString($semester["created"]);
    $semester_created = Globals::cSharpDateTimeToPHPDateTime($semester_created);
    $result = $connection->select("semester", ["user_id"], "`id` = {$semester_id}");
    if (sizeof($result) > 0) {
        $result = $result[0];
        $existing_user_id = $result["user_id"];
        if ($existing_user_id != $user_id)
            throw new InsufficientPermissionsException(); // user tried to update semester that wasnt his
        $connection->query("UPDATE `semester` SET `name` = '{$semester_name}', `created` = '{$semester_created}' WHERE `id` = {$semester_id}");
        $result = ["result" => "ok", "semesterID" => $semester_id];
    } else {
        $rows = [];
        $rows[] = ["user_id" => $user_id, "name" => $semester_name, "created" => $semester_created];
        $ids = $connection->insert("semester", $rows);
        if (sizeof($ids) == 0)
            throw new ServerException(); // insert query failed
        $semester_id = $ids[0];
        $result = ["result" => "ok", "semesterID" => $semester_id];
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