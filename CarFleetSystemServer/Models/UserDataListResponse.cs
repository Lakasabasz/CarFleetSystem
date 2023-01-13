﻿namespace CarFleetSystemServer.Models;

public class UserDataListResponse: Response
{
    public IEnumerable<UserData> Users;

    public UserDataListResponse(string errorDescription, int errorCode) : base(errorDescription, errorCode){ }
    public UserDataListResponse() : base("", 0){}
}