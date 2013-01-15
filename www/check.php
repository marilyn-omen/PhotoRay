<?php

session_start();
$filename = 'photos/'.session_id();
if(!file_exists($filename)) {
	echo -1;
} else {
	echo filectime($filename);
}

?>