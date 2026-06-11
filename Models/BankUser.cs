public class BankUser
{
    public string user { get; set; }
    public string passw { get; set; }
    public string role { get; set; }
    public double? money { get; set; }
    public string cardno { get; set; }
    public double? atmbal { get; set; }
    public double? loanmoney { get; set; }
    public string status { get; set; }  
    public BankUser(string usern, string pass, string r)
    {
        user = usern;
        passw = pass;
        role = r;

        cardno = null;
        atmbal = null;
        loanmoney = null;
        money = null;
        status = null;  
    }
}
