<?php
require_once("DBConnection.php");


class Globals
{
    const savePath = "saves/";
    private static $dbConnection = null;

    static function dateTimeNow() : string {
        return date("Y-m-d H:i:s", time());
    }
        
    static function dateDifference(string $date1, string $date2) : int {
        $t1 = strtotime($date1);
        $t2 = strtotime($date2);
        return abs($t1 - $t2);
    }

    static function cSharpDateTimeToPHPDateTime(string $dt) : string {
        $dt = str_replace("T", " ", $dt);
        $ms_position = strpos($dt, ".");
        if ($ms_position)
            $dt = substr($dt, 0, $ms_position);
        return $dt;
    }

    static function parseJson(string $json) : array {
        $json = str_replace("\\", "", utf8_decode($json));
        $json = str_replace("?", "", $json);
        $json = json_decode($json, true);
        if ($json === null)
            throw new WrongFormatException(); // json was invalid
        return $json;
    }

    static function hashAndSalt(string $password, int $salt) : string {
        return hash("sha256", $password . $salt);
    }

    static function intToTestType(int $t) : string {
        switch ($t) {
            case 0:
                return "exam";
                break;
            case 1:
                return "vocabulary_test";
                break;
            case 2:
                return "test";
                break;
        }
        return "";
    }

    static function testTypeToInt(string $type) : int {
        switch ($type) {
            case "exam":
                return 0;
                break;
            case "vocabulary_test":
                return 1;
                break;
            case "test":
                return 2;
                break;
        }
        return -1;
    }

    static function GetDBConnection() : DBConnection {
        if (Globals::$dbConnection == null)
            Globals::$dbConnection = new DBConnection("127.0.0.1", "root", "", "mynote");
        return Globals::$dbConnection;
    }
}

