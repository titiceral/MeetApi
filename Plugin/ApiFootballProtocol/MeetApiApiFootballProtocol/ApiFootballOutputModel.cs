using MeetApi.MeetApiInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace MeetApiApiFootballProtocol
{
    /*
    "{\"api\":
    {\"results\":4,
    \"leagues\":
[{\"league_id\":357,\"name\":\"Serie A\",
        \"type\":\"League\",
        \"country\":\"Brazil\",\"country_code\":\"BR\",\"season\":2019,\"season_start\":\"2019-04-27\",\"season_end\":\"2019-12-06\",\"logo\":\"Not available in demo\",\"flag\":\"https:\\/\\/media.api-football.com\\/flags\\/br.svg\",\"standings\":1,\"is_current\":1,\"coverage\":{\"standings\":true,\"fixtures\":{\"events\":true,\"lineups\":true,\"statistics\":true,\"players_statistics\":true},\"players\":true,\"topScorers\":true,\"predictions\":true,\"odds\":false}},{\"league_id\":524,\"name\":\"Premier League\",\"type\":\"League\",\"country\":\"England\",\"country_code\":\"GB\",\"season\":2019,\"season_start\":\"2019-08-09\",\"season_end\":\"2020-05-17\",\"logo\":\"Not available in demo\",\"flag\":\"https:\\/\\/media.api-football.com\\/flags\\/gb.svg\",\"standings\":1,\"is_current\":1,\"coverage\":{\"standings\":true,\"fixtures\":{\"events\":true,\"lineups\":true,\"statistics\":true,\"players_statistics\":true},\"players\":true,\"topScorers\":true,\"predictions\":true,\"odds\":false}},{\"league_id\":530,\"name\":\"Champions League\",\"type\":\"Cup\",\"country\":\"World\",\"country_code\":null,\"season\":2019,\"season_start\":\"2019-06-25\",\"season_end\":\"2019-12-11\",\"logo\":\"Not available in demo\",\"flag\":null,\"standings\":1,\"is_current\":1,\"coverage\":{\"standings\":true,\"fixtures\":{\"events\":true,\"lineups\":true,\"statistics\":true,\"players_statistics\":true},\"players\":true,\"topScorers\":true,\"predictions\":true,\"odds\":false}},{\"league_id\":566,\"name\":\"Eredivisie\",\"type\":\"League\",\"country\":\"Netherlands\",\"country_code\":\"NL\",\"season\":2019,\"season_start\":\"2019-05-10\",\"season_end\":\"2020-05-10\",\"logo\":\"Not available in demo\",\"flag\":\"https:\\/\\/media.api-football.com\\/flags\\/nl.svg\",\"standings\":1,\"is_current\":1,\"coverage\":{\"standings\":true,\"fixtures\":{\"events\":true,\"lineups\":true,\"statistics\":true,\"players_statistics\":true},\"players\":true,\"topScorers\":true,\"predictions\":true,\"odds\":false}}],\"version\":\"v2\",\"warning\":\"This is a demo and does not represent the entire API. The data is limited and not up to date and serves only as an example. for production environement use : https:\\/\\/api-football-v1.p.rapidapi.com\\/v2\\/\"}}"
*/
        public class ApiFootballOutputModel : IMeetApiProtocolOutputModel
    {
        public RootApiFootballModel api { get; set; }
    }

    public class RootApiFootballModel
    {
        public List<LeaguesApiFootballModel> leagues { get; set; }
    }

    public class LeaguesApiFootballModel
    {
        public virtual int league_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        [JsonPropertyName( "country")] // transcription pour la conversion initiale
        [JsonProperty(PropertyName = "pays")] // transcription pour la restitution


        public string country { get; set; }
    }
}
