
using static Const;
using static Functions;

Directory.CreateDirectory("./log");

string sqlServerName;
string sqlNameDB;
string sqlUserName;
string sqlPassword;
string ldapPath;
string adTagMailIgnor;
string company;
string streetAddress;
string city;
string telephoneNumber;
string website;
string b2c;
string websiteLink;
string imgLink;

using (StreamReader ReaderObject = new StreamReader(configFilelway))
{
    string[] fileData = new string[] { };
    fileData = ReaderObject.ReadToEnd().Split('\n');
    sqlServerName = GetParametrValue(fileData[0], " ")[1];
    sqlNameDB = GetParametrValue(fileData[1], " ")[1];
    sqlUserName = GetParametrValue(fileData[2], " ")[1];
    sqlPassword = GetParametrValue(fileData[3], " ")[1];
    ldapPath = GetParametrValue(fileData[4], " ")[1];
    adTagMailIgnor = GetParametrValue(fileData[5], " ")[1];
    company = GetParametrValue(fileData[6], ": ")[1];
    streetAddress = GetParametrValue(fileData[7], ": ")[1];
    city = GetParametrValue(fileData[8], ": ")[1];
    telephoneNumber = GetParametrValue(fileData[9], ": ")[1];
    website = GetParametrValue(fileData[10], ": ")[1];
    b2c = GetParametrValue(fileData[11], ": ")[1];
    websiteLink = GetParametrValue(fileData[12], ": ")[1];
    imgLink = GetParametrValue(fileData[13], ": ")[1];
}

string connectToSQL =
$"Data Source={sqlServerName};" +
    $"Initial Catalog={sqlNameDB};" +
    $"User ID={sqlUserName};" +
    $"Password={sqlPassword};" +
    $"Encrypt=false;" +
    $"TrustServerCertificate=true";

string userUnsver = "";
while (userUnsver != "exit")
{
    Dictionary<string, string> userData = new Dictionary<string, string>
        {
            {"cn", "" },
            {"title", "" },
            {"company", "" },
            {"streetAddress", "" },
            {"l", "" },
            {"mail", "" },
            {"telephoneNumber", "" },
            {"sAMAccountName", "" },
            {"mobile", "" },
            {"pager", "" }
        };
    Console.WriteLine("||===================================================||");
    Console.WriteLine("|| Обновляем подпись, или формируем списки?          ||");
    Console.WriteLine("|| 1. Обновляем подпись                              ||");
    Console.WriteLine("|| 2. Форируем списки                                ||");
    Console.WriteLine("|| Вводи номер действия или напиши exit, чтобы выйти ||");
    Console.WriteLine("||===================================================||");
    Console.Write("|| ");
    userUnsver = Console.ReadLine();

    if (userUnsver == "1")
    {
        Console.WriteLine("||===================================================||");
        Console.WriteLine("|| Напиши логин юзера                                ||");
        Console.Write("|| ");
        userData["sAMAccountName"] = Console.ReadLine();
        Console.WriteLine("||===================================================||");

        userData = GetAdUserAtributs(ldapPath, userData["sAMAccountName"]);
        if (userData["company"] == null)
        {
            userData["company"] = company;
        }
        if (userData["streetAddress"] == null)
        {
            userData["streetAddress"] = streetAddress;
        }
        if (userData["l"] == null)
        {
            userData["l"] = city;
        }
        if (userData["pager"] != adTagMailIgnor && userData["sAMAccountName"] != "tyty")
        {
            string identity_id = GetSqlIdentityID(connectToSQL, userData["cn"], userData["mail"]);

            string signatureText = "";
            if (identity_id != null)
            {
                if (userData["mobile"] == null)
                {
                    signatureText = openDiv + openSpan + "С уважением," + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["cn"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["title"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["company"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["l"] + ", " + userData["streetAddress"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + "Mail:         " + closeSpan +
                        openSpan + openStrong + "<a href=\"mailto:" + userData["mail"] + "\">" + userData["mail"] + "</a>" + closeStrong + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + "Phone: " + telephoneNumber + " доп: " + userData["telephoneNumber"] + "  " + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + openStrong + "website     " + "<a href=\"https://" + website + "\" rel=\"noopener\">" + website + "</a>" + closeStrong + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + openStrong + "b2c          " + "<a href=\"https://" + b2c + "\" rel=\"noopener\">" + b2c + "</a>" + closeStrong + closeSpan + closeDiv + "\r\n" +
                        openDiv + closeDiv + "\r\n" + openDiv + "<a href=\"https://" + websiteLink + "\" rel=\"noopener noreferrer\">" + "<img src=\"https://" + imgLink + "\" />" + "</a>" + closeDiv;
                }
                else
                {
                    signatureText = openDiv + openSpan + "С уважением," + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["cn"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["title"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["company"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + userData["l"] + ", " + userData["streetAddress"] + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + "Mail:         " + closeSpan +
                        openSpan + openStrong + "<a href=\"mailto:" + userData["mail"] + "\">" + userData["mail"] + "</a>" + closeStrong + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + "Phone: " + telephoneNumber + " доп: " + userData["telephoneNumber"] + "  " + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + "Mobile:      " + userData["mobile"] + "  " + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + openStrong + "website     " + "<a href=\"https://" + website + "\" rel=\"noopener\">" + website + "</a>" + closeStrong + closeSpan + closeDiv + "\r\n" +
                        openDiv + openSpan + openStrong + "b2c          " + "<a href=\"https://" + b2c + "\" rel=\"noopener\">" + b2c + "</a>" + closeStrong + closeSpan + closeDiv + "\r\n" +
                        openDiv + closeDiv + "\r\n" + openDiv + "<a href=\"https://" + websiteLink + "\" rel=\"noopener noreferrer\">" + "<img src=\"https://" + imgLink + "\" />" + "</a>" + closeDiv;
                }

                if (identity_id != "" && GetSqlSignature(connectToSQL, identity_id) != signatureText)
                {
                    Console.WriteLine("|| Сформированная для identity_id=" + identity_id + " подпись не совпадает с полученной - перезаписываем.");
                    UpdateSqLSignature(connectToSQL, signatureText, identity_id);
                }
                else
                {
                    Console.WriteLine("|| У пользователя и так правильная подпись           ||");
                }
            }

        }
        else
        {
            if (userData["pager"] == adTagMailIgnor)
            {
                Console.WriteLine("Этому пользователю нельзя менять подпись, у него pager=" + adTagMailIgnor);
            }
            if (userData["sAMAccountName"] == "tyty")
            {
                Console.WriteLine("|| Нет такого пользователя, ты шо там накалякал?     ||");
            }
        }

        //Console.WriteLine("Хочешь выйти, пиши exit, хочешь повторить, пиши любую байду");
    }

    if (userUnsver == "2")
    {
        List<Dictionary<string, string>> userDataList = new List<Dictionary<string, string>>();
        List<string> adLoginList = new List<string>();
        adLoginList = CutNullData(GetAllLoginAd(ldapPath).ToArray());

        for (int i = 0; i < adLoginList.Count; i++)
        {
            Dictionary<string, string> userDataCopy = new Dictionary<string, string>(userData);
            userDataCopy["sAMAccountName"] = adLoginList[i];
            userDataList.Add(userDataCopy);
        }

        for (int i = 0; i < userDataList.Count; i++)
        {
            Dictionary<string, string> userDataSearch = GetAdUserAtributs(ldapPath, userDataList[i]["sAMAccountName"]);

            userDataList[i]["cn"] = userDataSearch["cn"];
            userDataList[i]["title"] = userDataSearch["title"];
            userDataList[i]["company"] = userDataSearch["company"];
            userDataList[i]["streetAddress"] = userDataSearch["streetAddress"];
            userDataList[i]["mail"] = userDataSearch["mail"];
            userDataList[i]["telephoneNumber"] = userDataSearch["telephoneNumber"];
            userDataList[i]["mobile"] = userDataSearch["mobile"];
            userDataList[i]["l"] = userDataSearch["l"];
            userDataList[i]["pager"] = userDataSearch["pager"];

        }

        userDataList.RemoveAll(dict => (dict["mail"] == null));

        UsingStreamWriter("||=============Email, которые следует пропустить==============||");
        UsingStreamWriter("||====================так как pager=" + adTagMailIgnor  + "====================||");
        for (int i = 0; i < userDataList.Count; i++)
        {
            if (userDataList[i]["pager"] == adTagMailIgnor)
            {
                UsingStreamWriter("cn=" + userDataList[i]["cn"]);
                UsingStreamWriter("mail=" + userDataList[i]["mail"]);
                UsingStreamWriter("");
            }
        }

        userDataList.RemoveAll(dict => (dict["pager"] == adTagMailIgnor));

        UsingStreamWriter("||====Выносим юзеров с одинаковыми mail в отдельный список====||");
        UsingStreamWriter("");
        var allDuplicates = userDataList
            .GroupBy(dict => dict["mail"])
            .Where(group => group.Count() > 1)
            .SelectMany(group => group)
            .ToList();
        for (int i = 0; i < allDuplicates.Count; i++)
        {
            UsingStreamWriter("cn=" + allDuplicates[i]["cn"]);
            UsingStreamWriter("mail=" + allDuplicates[i]["mail"]);
            UsingStreamWriter("");
        }

        for (int i = 0; i < allDuplicates.Count; i++)
        {
            userDataList.RemoveAll(dict => (dict["mail"] == allDuplicates[i]["mail"]));
        }

        UsingStreamWriter("||===============Список mail, которых нет в AD================||");
        List<string> mailFromSql = GetSqlAllMail(connectToSQL);
        for (int i = 0; i < mailFromSql.Count; i++)
        {
            bool x = false;
            for (int j = 0; j < userDataList.Count; j++)
            {
                if (mailFromSql[i] == userDataList[j]["mail"])
                {
                    x = true;
                }
            }
            if (x == false)
            {
                UsingStreamWriter(mailFromSql[i]);
            }
        }

        UsingStreamWriter("||===========У следующих mail не совпадает ФИО с AD===========||");
        for (int i = 0; i < userDataList.Count; i++)
        {
            FindeNameDoesntExistAd(connectToSQL, userDataList[i]["mail"], userDataList[i]["cn"]);
        }

        Console.WriteLine("|| Информацию ищи в файле log\\log.txt                ||");
    }

    if (userUnsver != "1" && userUnsver != "2" && userUnsver != "exit")
    {
        Console.WriteLine("||===================================================||");
        Console.WriteLine("|| Сам ты " + userUnsver);
        Console.WriteLine("|| Напиши 1 или 2, в зависимости от того, что хочешь ||");
        Console.WriteLine("|| сделать!                                          ||");
        Console.WriteLine("|| Чтобы выйти, напиши exit                          ||");
        Console.WriteLine("||===================================================||");
    }

    if (userUnsver == "exit")
    {

    }
    
}
