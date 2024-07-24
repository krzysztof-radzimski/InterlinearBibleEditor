if (File.Exists("app_online.htm")) {
    File.Move("app_online.htm", "app_offline.htm");
}
else if (File.Exists("app_offline.htm")) {
    File.Move("app_offline.htm", "app_online.htm");
}



if (File.Exists("../KOSCIOLJEZUSA.PL/app_online.htm")) {
    File.Move("../KOSCIOLJEZUSA.PL/app_online.htm", "../KOSCIOLJEZUSA.PL/app_offline.htm");
}
else if (File.Exists("../KOSCIOLJEZUSA.PL/app_offline.htm")) {
    File.Move("../KOSCIOLJEZUSA.PL/app_offline.htm", "../KOSCIOLJEZUSA.PL/app_online.htm");
}
