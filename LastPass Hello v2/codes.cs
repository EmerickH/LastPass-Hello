class Code
 {
    public Code(string username, string token)
    {
        this.Username = username;
        this.Token = token;
    }

    string _username = "";
    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }

    string _token = "";
    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

}
