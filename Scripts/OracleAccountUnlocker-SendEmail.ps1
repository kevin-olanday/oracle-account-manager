param($user,$recipients,$database,$reason,$initiator)
$initiator = $initiator.trimstart("x")
$email2 = ([adsisearcher]"name=$initiator").findone().Properties.mail
$name = ([adsisearcher]"name=$user").findone().Properties.givenname
$username = $name[0]

$subject = "[Oracle] Unlock Account -  $reason"
$body = "Hello ,</br></br>The account - <strong>$user</strong> - has been unlocked on the Oracle database <strong>$database</strong></br></br>Please note that the password on this account is not controlled by the Password Changer tool.</br></br>Any queries or problems contact the ITG Service Desk on Ext 54321.</br></br><i>This is an automated email.</i>"
$sender = "OracleAccountManager-ALICE@macquarie.com"
$recipients = $recipients.trim().split(";")
$recipient = @()
foreach($user in $recipients)
{
    $email = ([adsisearcher]"name=$user").findone().Properties.mail[0]
    $recipient += $email
}


if($email2)
{
  $bcc = $email2[0]
}

$SMTPServer = "smtpsrvc.lb.macbank"
$SMTPPort = "25"

    
Send-MailMessage -Subject $subject -Body $body -From $sender -to $recipient -bcc $bcc -SmtpServer $server -Port $SMTPPort -BodyAsHtml
