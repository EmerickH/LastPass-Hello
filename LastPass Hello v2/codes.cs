class code
 {
    public code(string username, string token)
    {
        this.username = username;
        this.token = token;
    }

    string _username = "";
    public string username
    {
        get { return _username; }
        set { _username = value; }
    }

    string _token = "";
    public string token
    {
        get { return _token; }
        set { _token = value; }
    }

}
