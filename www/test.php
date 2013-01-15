<?php
session_start();
?>
<html>
<body>
<form enctype="multipart/form-data" action="upload.php" method="POST">
<input name="sid" type="text" value="<?php echo session_id() ?>" />
<br />
Choose file: <input name="uploadedfile" type="file" />
<input type="submit" value="Upload" />
</form>
</body>
</html>