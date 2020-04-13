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

    if (!(array_key_exists("auth", $post) && array_key_exists("userID", $post) && array_key_exists("courseID", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $user_id_to_get = $connection->realEscapeString($post["userID"]);
    $course_id = $connection->realEscapeString($post["courseID"]);

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $result = $connection->select("user_in_course", ["user_id", "admin"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}'");
    if (sizeof($result) === 0)
        throw new InsufficientPermissionsException(); // user wasn't in requested course

    $result = $result[0];
    $admin = $result["admin"];
    $admin = $admin == 1 ? true : false;

    if ($admin || $user_id == $user_id_to_get) {
        $connection->query("DELETE FROM `user_in_course` WHERE `user_id` = {$user_id_to_get} AND `course_id` = {$course_id}");
        $result = $connection->select("user_in_course", ["user_id"], "`user_id` = '{$user_id}' AND `course_id` = '{$course_id}' AND `admin` = 1");
        if (sizeof($result) === 0) {
            $result = $connection->select("user_in_course", ["user_id"], "`course_id` = '{$course_id}'", "`member_since` ASC");
            if (sizeof($result) > 0) {
                $result = $result[0];
                $new_admin_id = $result["user_id"];
                $connection->query("UPDATE `user_in_course` SET `admin` = 1 WHERE `course_id` = '{$course_id}' AND `user_id` = '{$new_admin_id}'");
            }
        }

        return ["result" => "ok"];
    }
    throw new InsufficientPermissionsException();
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
