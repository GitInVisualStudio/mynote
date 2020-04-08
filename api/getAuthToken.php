<?php
require_once("Utils/DBConnection.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/WrongCredentialsException.php");

function main() : array {
    $connection = new DBConnection("127.0.0.1", "root", "", "mynote");
    
    if (!(array_key_exists("username", $_POST) || array_key_exists("password", $_POST)))
        throw new MissingParameterException();
    
    $username = $connection->realEscapeString($_POST["username"]);
    $password = $connection->realEscapeString($_POST["password"]);
    $result = $connection->select("user", ["salt"], "`username` = '{$username}'");
    if (empty($result))
        throw new WrongCredentialsException();
    $salt = $result[0]["salt"];
    $hash = hash("sha256", $password . $salt);
    $result = $connection->select("user", ["id"], "`username` = '{$username}' AND `password` = '{$hash}'");
    if (sizeof($result) != 1)
        throw new WrongCredentialsException();
    
    $user_id = $result[0]["id"];
    $dt = date("Y-m-d H:i:s", time());
    $token = hash("sha256", $username . time());
    $connection->query("UPDATE user SET `auth` = '{$token}', `auth_since` = '{$dt}' WHERE `id` = '{$user_id}'");
    $result = ["result" => "ok", "token" => $token];
    
    return $result;
}

try {
    $array = main();
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    