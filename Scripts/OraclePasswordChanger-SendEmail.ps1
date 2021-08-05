param($user,$password,$database,$reason, $initiator)
$initiator = $initiator.trimstart("x")
$user = $user.replace("OPS$","").replace("_USR","")
$email = ([adsisearcher]"name=$user").findone().Properties.mail 
$name = ([adsisearcher]"name=$user").findone().Properties.givenname
$username = $name[0]


$subject = "[Oracle] Reset Database Password -  $reason"
$body = "Hello <strong>$username</strong>,</br></br>Your password on Oracle database <strong>$database</strong> has been reset.</br></br><strong>Login ID:</strong>   $user</br><strong>Password:</strong>   $password</br></br>Please note that the password on this account is not controlled by the Password Changer tool.</br></br>Any queries or problems contact the ITG Service Desk on Ext 54321.</br></br><i>This is an automated email.</i>"
$sender = "OracleAccountManager-ALICE@macquarie.com"
if($email)
{
  $recipient = $email[0]
}

$SMTPServer = "smtpsrvc.lb.macbank"
$SMTPPort = "25"
  

$email | out-file c:\temp\email.txt
Send-MailMessage -Subject $subject -Body $body -From $sender -to $recipient  -SmtpServer $server -BodyAsHtml
if($?)
{
   $body = "Password for $user on $database was successfully reset. A notification was sent to the user."
   $email = ([adsisearcher]"name=$initiator").findone().Properties.mail 
   if($email)
   {
    $recipient = $email[0]
   }   
   Send-MailMessage -Subject $subject -Body $body -From $sender -to $recipient -bcc $bcc -SmtpServer $server -Port $SMTPPort -BodyAsHtml
}
else
{

   $body = "Password for $user on $database was successfully reset. Failed to sent notification to the user."
   $email = ([adsisearcher]"name=$initiator").findone().Properties.mail 
   if($email)
   {
    $recipient = $email[0]
   }   
   Send-MailMessage -Subject $subject -Body $body -From $sender -to $recipient -bcc $bcc -SmtpServer $server -Port $SMTPPort -BodyAsHtml
}

