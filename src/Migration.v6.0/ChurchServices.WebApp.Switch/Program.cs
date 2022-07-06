if (File.Exists("app_online.htm")) {
    File.Move("app_online.htm", "app_offline.htm");
}
else if (File.Exists("app_offline.htm")) {
    File.Move("app_offline.htm", "app_online.htm");
}
