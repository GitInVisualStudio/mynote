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
    $connection = Globals::GetDBConnection();

    if (!(array_key_exists("auth", $post) && array_key_exists("canvas", $post)))
        throw new MissingParameterException();

    $auth = $connection->realEscapeString($post["auth"]);
    $canvas = $post["canvas"];

    $result = $connection->select("user", ["id", "auth_since"], "`auth` = '{$auth}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException(); // auth token doesnt exist

    $user_id = $result[0]["id"];
    $auth_since = $result[0]["auth_since"];
    $days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
    if ($days >= 1)
        throw new AuthTokenExpiredException(); // auth token expired

    $canvas_id = $connection->realEscapeString($canvas["onlineID"]);
    $course_id = $connection->realEscapeString($canvas["courseOnlineID"]);
    $canvas_name = $connection->realEscapeString($canvas["name"]);
    $canvas_created = $connection->realEscapeString($canvas["dt"]);
    $canvas_created = Globals::cSharpDateTimeToPHPDateTime($canvas_created);
    $canvas["created"] = $canvas_created;
    $canvas_type = $canvas["type"];
    switch ($canvas_type) {
        case "MyNoteBase.Canvasses.Note":
            $canvas_type = "note";
            break;

        case "MyNoteBase.Canvasses.VocabularyListing":
            $canvas_type = "vocabulary_listing";
            break;

        case "MyNoteBase.Canvasses.Excercise":
            $canvas_type = "excercise";
            break;
    }

    unset($canvas["localID"]);
    unset($canvas["courseLocalID"]);

    $result = $connection->select("course", ["id"], "`id` = '{$course_id}'");
    if (sizeof($result) == 0)
        throw new InvalidHigherIDException(); // given course was not valid
    else if (sizeof($result) > 1)
        throw new ServerException(); // two semesters with same id existed (shouldn't happen)

    $result = $connection->select("canvas", ["user_id", "path"], "`id` = '{$canvas_id}'");
    if (sizeof($result) > 0) {
        $result = $result[0];
        $existing_user_id = $result["user_id"];
        $path = $result["path"];
        if ($existing_user_id != $user_id)
            throw new InsufficientPermissionsException(); // user tried to update canvas that wasn't theirs
        $connection->query("UPDATE `canvas` SET `course_id` = '{$course_id}', `name` = '{$canvas_name}', `created` = '{$canvas_created}', `type` = '{$canvas_type}' WHERE `id` = {$canvas_id}");
        $result = ["result" => "ok", "canvasID" => $canvas_id];
    } else {
        $rows = [];
        $rows[] = ["user_id" => $user_id, "course_id" => $course_id, "name" => $canvas_name, "created" => $canvas_created, "type" => $canvas_type];
        $ids = $connection->insert("canvas", $rows);
        if (sizeof($ids) == 0)
            throw new ServerException(); // insert query failed
        $canvas_id = $ids[0];
        $canvas["onlineID"] = $canvas_id;
        $path = Globals::savePath . "canvasses/" . $canvas_id . ".myc";
        $connection->query("UPDATE `canvas` SET `path` = '{$path}' WHERE `id` = '{$canvas_id}'");
        $result = ["result" => "ok", "canvasID" => $canvas_id];
    }

    $dir = pathinfo($path, PATHINFO_DIRNAME);
    if (is_file($dir))
        throw new ServerException();
    if (!is_dir($dir))
        mkdir($dir, 0777, true);
    file_put_contents($path, json_encode($canvas));
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
