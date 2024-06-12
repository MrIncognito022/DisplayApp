namespace DisplayApp.Models
{
    public class PhoneDataModel
    {
        public Data data { get; set; }
        public Response[] responses { get; set; }
    }

    public class Data
    {
        public string dnc { get; set; }
        public string alert_locked { get; set; }
        public string codec { get; set; }
        public string bnumber { get; set; }
        public string delivery { get; set; }
        public string emergency_address_code { get; set; }
        public string unumber_group { get; set; }
        public string utype { get; set; }
        public string dtype { get; set; }
        public string dialplan { get; set; }
        public string dnumber { get; set; }
        public string plan { get; set; }
        public string provision1 { get; set; }
        public string timezone { get; set; }
        public string region { get; set; }
        public string btype { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string emergency_address2 { get; set; }
        public string ltype { get; set; }
        public string emergency_address3 { get; set; }
        public string ringtone { get; set; }
        public string ping { get; set; }
        public string rpid { get; set; }
        public string emergency_location_code { get; set; }
        public string source { get; set; }
        public string lastcaller { get; set; }
        public string callerid_location { get; set; }
        public string missedemail { get; set; }
        public string hardware_address { get; set; }
        public string language { get; set; }
        public string dnd { get; set; }
        public string screen { get; set; }
        public string presentation_external { get; set; }
        public string recordgroup { get; set; }
        public string mailbox { get; set; }
        public string expires { get; set; }
        public string lnumber { get; set; }
        public string pin { get; set; }
        public string emergency_uploaded { get; set; }
        public string forwarding { get; set; }
        public string alert_registered { get; set; }
        public string callerid_external { get; set; }
        public string emergency_address_other { get; set; }
        public string display { get; set; }
        public string lastcalled { get; set; }
        public string media { get; set; }
        public string callername { get; set; }
        public string lastcallertime { get; set; }
        public string transaction { get; set; }
        public string music { get; set; }
        public string lastprovision_time { get; set; }
        public string id { get; set; }
        public string lastdndtime { get; set; }
        public string provisioning { get; set; }
        public string registrar { get; set; }
        public string expect_registered { get; set; }
        public string ringtime { get; set; }
        public string unumber { get; set; }
        public string direct { get; set; }
        public string lphone { get; set; }
        public string emergency_address1 { get; set; }
        public string callerid_internal { get; set; }
        public string emergency_address_city { get; set; }
        public string cos { get; set; }
        public string emergency_updated { get; set; }
        public string music_ringing { get; set; }
        public string owner { get; set; }
        public string acr { get; set; }
        public string failed_count { get; set; }
        public string lastprovision_from { get; set; }
        public string totalmaximum { get; set; }
        public string clidblock { get; set; }
        public string emergency_country { get; set; }
        public string lastcalledtime { get; set; }
        public string emergency_address_state { get; set; }
        public string customer { get; set; }
    }

    public class Response
    {
        public string message { get; set; }
        public string key { get; set; }
        public int code { get; set; }
    }
}
