<?php
require_once("Utils/DBConnection.php");
require_once("Utils/Globals.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/AuthTokenExpiredException.php");

function main() : array {
    $connection = new DBConnection("127.0.0.1", "root", "", "mynote");
    
    if (!(array_key_exists("auth", $_POST) && array_key_exists("semester", $_POST)))
        throw new MissingParameterException();
    
    $auth = $_POST["auth"];
    
    try {
        $semester = json_decode($_POST["semester"], true);
    } catch (Exception $e) {
        throw new WrongFormatException();
    }
    
    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException();
    
    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference(date_create($auth_since), date_create(Globals::dateTimeNow()))["days"];
    if ($days >= 1)
        throw new AuthTokenExpiredException();
    
    $result = ["result" => "ok", "semesterID" => 1];
    return $result;
}

try {
    $array = main();
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);