using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using System.Linq;

namespace Common.Http
{
    public class Strana
    {
        protected string adresa;
        string sadrzaj;
        DateTime vreme;
        public string Adresa { get { return adresa; } }
        public string Sadrzaj { get { return sadrzaj; } }
        public DateTime Vreme { get { return vreme; } }

        public Strana(string adresa)
        {
            this.adresa = adresa;
            sadrzaj = null;
        }

        public virtual bool Procitaj()
        {
            if (Properties.Settings.Default.MaxBrojCitanjaStrane >= 0)
            {
                int vremeCekanja = Properties.Settings.Default.VremeCekanjaNaPonovnoCitanjeStrane;
                sadrzaj = string.Empty;
                List<Exception> izuzeci = new List<Exception>();
                int brojPokusaja;
                for (brojPokusaja = 0; brojPokusaja < Properties.Settings.Default.MaxBrojCitanjaStrane; brojPokusaja++)
                {
                    string greska = string.Empty;
                    try
                    {
                        sadrzaj = HttpComm.GetPage(adresa).ToString();

                        // zbog testa
                        /*if (this is StranaZaglavlja)
                        {
                            sadrzaj = TestZaglavlje();
                        }
                        else
                        {
                            sadrzaj = TestOglas();
                        }*/

                        vreme = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(sadrzaj))
                        {
                            Dnevnik.PisiSaImenomThreda("Strana je procitana. URL: " + adresa);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        greska = string.Format(" Exception: {0}. StackTrace: {1}", ex.Message, ex.StackTrace);
                    }
                    sadrzaj = string.Empty;
                    Dnevnik.PisiSaThredomGreska(string.Format("Neuspelo čitanje strane sa adrese {0}. Pokušaj {1}/{2}.", adresa, brojPokusaja + 1, Properties.Settings.Default.MaxBrojCitanjaStrane) + greska);
                    System.Threading.Thread.Sleep(vremeCekanja);
                    vremeCekanja *= 2;
                }
                if (brojPokusaja == Properties.Settings.Default.MaxBrojCitanjaStrane)
                {
                    string poruka = string.Format("Strana nije procitana iz {0} pokusaja. URL: {1}", Properties.Settings.Default.MaxBrojCitanjaStrane, adresa);
                    Dnevnik.PisiSaThredomGreska(poruka);
                    //throw new Exception(poruka);
                }
                return false;
            }
            else
                throw new Exception("Definisanji broj citanja strane je nula.");
        }

        public bool JeZaglavlje()
        {
            return adresa.Contains("http://www.polovniautomobili.com/putnicka-vozila-26/");
        }

        public bool JeOglas()
        {
            return adresa.Contains("http://www.polovniautomobili.com/oglas");
        }

        public List<string> DajAdreseOglasa()
        {
            HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
            d.LoadHtml(Sadrzaj);
            List<string> adrese = new List<string>();
            HtmlAgilityPack.HtmlNodeCollection nodeCol;
            nodeCol = d.DocumentNode.SelectNodes("id('searchlist-items')");
            if (nodeCol != null)
            {
                foreach (HtmlNode n in nodeCol[0].ChildNodes.Where(node => node.Name.ToLower().Equals("li")))
                {
                    if ((n.Attributes.Count == 1))
                    {
                        if (n.ChildNodes.Count >= 2 &&
                            n.ChildNodes[1].ChildNodes.Count >= 2 &&
                            n.ChildNodes[1].ChildNodes[1].ChildNodes.Count >= 2 &&
                            n.ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes.Count >= 2)
                        { 
                            adrese.Add("http://www.polovniautomobili.com" + n.ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes[1].Value);
                        }
                    }
                }
                return adrese;
            }
            else
                return null;
        }

        /// <summary>
        /// Samo za potrebe testa. Vraca uvek istu html stranu.
        /// </summary>
        /// <returns></returns>
        private string TestZaglavlje()
        {
            return @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" lang=""SR"" xml:lang=""SR"">
<head>
	<title>Polovni automobili -  auto oglasi, prodaja automobila, vozila, auto placevi, kola, motori, kamioni, auto delovi</title>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
	<meta name=""keywords"" content=""prodaja polovnih automobila, polovni automobili prodaja, vozila, automobili, auto oglasi, auto placevi, auto plac, auto delovi, audi, bmw, alfa romeo, fiat, opel, peugeot, renault, mercedes benz, citroen, volkswagen, ford, vozilo, kola, automobil, kombi, motori, kamioni, polovni automobili beograd, automobili kupovina, polovni automobili kredit, polovni automobili iz uvoza"" />
	<meta name=""description"" content=""Najposećeniji sajt za polovne i nove automobile u Srbiji. Kupite ili prodajte auto putem besplatnih oglasa. Pronađite pravi auto za sebe."" />
	<meta http-equiv=""X-UA-Compatible"" content=""IE=9"" />
	<meta http-equiv=""content-language"" content=""SR"">
	<meta name=""google-site-verification"" content=""9-7CPBCpKIMIbGvlGecaVh4sOTiPHORKoCzrywaFb88"" />
	<meta name=""alexaVerifyID"" content=""BlqQz4Lk4Lu0a3xuQEBI9g6jB5o"" />
	<meta property=""fb:page_id"" content=""36224102230"" />
	<meta property=""fb:app_id"" content=""127593208027""/>
	<meta property=""og:image"" content=""http://www.polovniautomobili.com/images/polovniautomobili.com-logo.gif"" />
	<meta property=""og:title"" content=""Polovni automobili -  auto oglasi, prodaja automobila, vozila, auto placevi, kola, motori, kamioni, auto delovi"" />
	<meta property=""og:type"" content=""website"" />
	<meta property=""og:url"" content=""http://www.polovniautomobili.com/putnicka-vozila-26.php?category=26&tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" />
	<meta property=""og:site_name"" content=""www.polovniautomobili.com"" />

	
	
	<script	type=""text/javascript"">
	var	PageTimerStart = new Date().getTime();
	</script>


	<link type=""text/css"" rel=""stylesheet"" media=""all"" href=""http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/themes/ui-lightness/jquery-ui.css"" />
	<link type=""text/css"" rel=""stylesheet"" media=""all"" href=""/css/jquery-ui-newpa/jquery-ui-1.10.0.custom.min.css"" />
	<script type=""text/javascript"" src=""//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js""></script>
	<script type=""text/javascript"" src=""//ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js""></script>
	
	<link type=""text/css"" rel=""stylesheet"" media=""screen"" href=""/css/pa.php"" />
	
	<link type=""text/css"" rel=""stylesheet"" media=""screen"" href=""/css/modalbox.php"" />
	
	<link type=""text/css"" rel=""stylesheet"" media=""screen"" href=""/css/jqui.php"" />
	
	<link type=""text/css"" rel=""stylesheet"" media=""print"" href=""/css/print.php"" />
	
	<script type=""text/javascript"" src=""/javascript.php?""></script>
	
	<!-- GA code / -->

	<script type=""text/javascript"">
		var _gaq = _gaq || [];
		_gaq.push(['_setAccount', 'UA-220728-1']);
		_gaq.push(['_trackPageview']);

		(function()
			{
			var ga = document.createElement('script');
			ga.type = 'text/javascript';
			ga.async = true;
			ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
			var s = document.getElementsByTagName('script')[0];
			s.parentNode.insertBefore(ga, s);
			})();
	</script>

	<script type=""text/javascript"">
		jQuery(document).ready(function()
		{
		jQuery("".megamenu"").megamenu({
			'show_method':'simple',
			'hide_method': 'fadeOut',
			'enable_js_shadow':false
		});
		});
	</script>

	
	
	<script type=""text/javascript"">
	var banner_check = setInterval(function() {
		//console.log('checking banner..');
		if( jQuery('div[id^=""httpool_hlpContainer_""]').css('width') == '600px' && jQuery('div[id^=""httpool_hlpContainer_""]').css('height') == '250px' && jQuery('div[id^=""httpool_hlpContainer_""]').css('zIndex') != 'auto' ) {
			jQuery('div[id^=""httpool_hlpContainer_""]').css('zIndex', 'auto');
			//console.log('banner set');
			clearInterval(banner_check);
		}
	}, 1000);
	</script>
		
	<script type=""text/javascript"">
	var banner_check2 = setInterval(function() {
		//console.log('checking banner..');
		if( jQuery('div[id$=""_exp.inside_out""]').css('width') == '728px' && jQuery('div[id$=""_exp.inside_out""]').css('height') == '90px' && jQuery('div[id$=""_exp.inside_out""]').css('zIndex') != 24 ) {
			jQuery('div[id$=""_exp.outside""]').css('zIndex', 24);
			jQuery('div[id$=""_exp.inside_out""]').css('zIndex', 24);
			//console.log('banner set');
			clearInterval(banner_check2);
		}
	}, 1000);
	</script>

</head>

<body lang=""SR""


onunload=""unloadCallback(); ""  data-ip=""95.180.20.182"" data-user_id=""30101"">


<script type=""text/javascript"">
<!--//--><![CDATA[//><!--
var pp_gemius_identifier = new String('bDE6tpAOi_0zLDtsGnWH_HamT.QNHUPZGpu6CxYFf.r.F7');
//--><!]]>
</script>
<script type=""text/javascript"" src=""http://www.polovniautomobili.com/javascript/xgemius.js""></script>


<div id=""fb-root""></div>
<script type=""text/javascript"">
// Load Facebook JS SDK asynchronously
window.fbAsyncInit = function() {
	FB.init({
		appId  : '127593208027',
		status : true, // check login status
		cookie : true, // enable cookies to allow the server to access the session
		xfbml  : true  // parse XFBML
		});

    /*FB.getLoginStatus(function(response) {
        if (response.status == 'unknown' && jQuery('#information iframe').length)
           {
           jQuery('#information iframe').css('height',208).parent().css('height',208);
           }
    }, true);*/
};
(function() {
	var e = document.createElement('script');
	e.src = document.location.protocol + '//connect.facebook.net/sr_RS/all.js';
	e.async = true;
	document.getElementById('fb-root').appendChild(e);
}());
</script>


<!-- JavaScript tag: PA pozadinski KOD unutrasnje, 7000606 -->
<script type=""text/javascript"">
// <![CDATA[
    var ord=Math.round(Math.random()*100000000);
    document.write('<sc'+'ript type=""text/javascript"" src=""http://ad.adverticum.net/js.prm?zona=7000606&ord='+ord+'""><\/scr'+'ipt>');
// ]]>
</script>
<noscript><a href=""http://ad.adverticum.net/click.prm?zona=7000606&nah=!ie"" target=""_blank"" title=""Click on the advertisement!""><img border=""0"" src=""http://ad.adverticum.net/img.prm?zona=7000606&nah=!ie"" alt=""Advertisement"" /></a></noscript>

<div class=""float-banner-left"">
	<!-- Goa3 tag: PA pozadinski unutrasnje LEVO, 7000603 -->
	<div id=""zone7000603"" class=""goAdverticum""></div>
</div>
<div class=""float-banner-right"">
	<!-- Goa3 tag: PA pozadinski unutrasnje DESNO, 7000604 -->
	<div id=""zone7000604"" class=""goAdverticum""></div>
</div>




<a name=""MountEverest""></a>

<div id=""waitingAjaxRequest"" style=""display: none; text-align: center;"">
	<br /><img src=""/images/bigrotation2x.gif"" alt=""Učitavanje"" /><br />- Učitavanje -
</div>
<div id=""overlaybox"" style=""display:none;"">
	<div id=""msgOverlay""></div>
	<div id=""msgClose"" class=""MB_done""><a href=""#"" id=""overlayboxClose"" onclick=""Modalbox.hide(); return false;"">Zatvori</a>
	</div>
</div>
<div id=""overlayConfirm"" style=""display: none;"">
	<div id=""msgConfirm""></div>
	<div id=""msgYesNo"" class=""MB_done""><input type=""button"" name=""overlayConfirmYes"" value=""Da"" class=""button"" />
		<input type=""button"" value=""Ne"" onclick=""Modalbox.hide(); return false;"" class=""button"" /></div>
</div>
<div id=""adminStatusReport"" style=""display: none;""></div>

<div id=""loginboxoverlay"" style=""display:none;"">
	<h2>Prijava</h2>

	<form class=""fullpage-form"" method=""post"" action="""">
		<fieldset class=""overlay"">
			<input type=""hidden"" id=""logintype"" value=""login"" />
			<div id=""errorlogin"" class=""error""></div>
			<div class=""set"">
				<label for=""username"">E-mail:</label>
				<input type=""text"" class=""text"" id=""username"" name=""email"" value="""" />
			</div>
			<div class=""set"">
				<label for=""password"">Šifra:</label>
				<input type=""password"" class=""text"" id=""password"" value="""" onkeyup=""if(event.keyCode == 13) tryToLogin('overlay-enter');"" />
			</div>
			<div class=""set"">
				<label for=""rememberme""><input type=""checkbox"" id=""rememberme"" class=""input_checkbox"" value=""yes"" />
					Zapamti me</label>
			</div>
            <table>
                <tr>
                    <td>
                        <div class=""set buttons"">
                            <input type=""button"" class=""button submit-button"" name=""submit"" value=""Prijava"" onclick=""$('errorlogin').style.display='none';tryToLogin('overlay-click'); var element = $('errorlogin'); new Effect.Appear(element); return false;"" />
                        </div>
                    </td>

                    <td>
                        <div>
                            <a href=""https://www.facebook.com/dialog/oauth?client_id=127593208027&redirect_uri=http%3A%2F%2Fwww.polovniautomobili.com%2Fmoj-profil.php%3Flogin%3Dfacebook%26fbloggedlink%3Dtrue&state=7d0749d462f92ef9ac5e89394f5d92b5&scope=email%2Cuser_location"">
                                <img src=""/images/fblogin-button.png"" style=""height:24px; width:101px; margin:-1px 0 0 -3px;"" />
                            </a>
                        </div>
                    </td>
                </tr>
            </table>
			<div class=""set"">
				Novi korisnik? <a href=""/registracija.php"">Registruj se!</a> |
				<a href=""#"" onclick=""Modalbox.hide(); Modalbox.show($('lostPassOverlay'));return false;"">Zaboravljena šifra?</a><br />
			</div>
			<div class=""set"">
				<a href=""#"" class=""close-overlay"" onclick=""Modalbox.hide(); return false;"">Odustani</a>
			</div>
		</fieldset>
	</form>
</div>

<div id=""main"" class=""extend"">

<div id=""service_links_row"" style=""width:1002px; height:20px; padding-top:4px;"">
	<div id=""service_links"" style=""float:left;font-size:11px;"">
		<ul>
			<li><a href=""/oglasi-na-mail.php"" title=""Primajte na vaš e-mail novopostavljene oglase vozila, vesti iz oblasti automobilizma i novosti na sajtu"" class="""">Oglasi na e-mail</a></li>
			<li><a href=""/knjiga-utisaka.php"" title=""Ostavite komentar na rad sajta i prenesite nam vaš opšti utisak"" class="""">Knjiga utisaka</a></li>
			<li><a href=""/wiki/cesto-postavljana-pitanja.php"" title=""Česta pitanja posetilaca"" class="""">Česta pitanja</a>
			</li>
			<li><a href=""http://www.infostud.com/za-medije/polovni-automobili/"" title=""Za medije"" class="""" target=""_blank"">Za medije</a>
			</li>
			<li><a href=""http://www.infostud.com/ponuda-za-predstavljanje-putem-banera-na-sajtu-polovniautomobili"" title=""Postavite baner na PolovniAutomobili.com"" class="""" target=""_blank"">Baneri</a></li>
			<li><a href=""/wiki/o-nama.php"" title=""Nešto više o nama..."" class="""">O nama</a></li>
			<li><a href=""/wiki/rss-i-mobilne-aplikacije.php"" title=""RSS sadržaj i aplikacija za Nokia mobilne telefone"" class="""">RSS i mob aplikacije</a>
			</li>
			<li>
				<a href=""/wiki/korisni-linkovi.php"" title=""Korisni linkovi - adrese sajtova koje preporučujemo"" class="""">Korisni linkovi</a>
			</li>
		</ul>
	</div>
	<div id=""loggedinbox"" style=""display: block;"">
		Dobrodošli  
		<a href=""/moj_profil.php"" rel=""nofollow"" style=""font-weight:bold"" title=""Kliknite ovde za pregled profila"">nemanja.simovic@gmail.com</a>
		<span class=""delimiter"">|</span>
		<a href=""#"" onclick=""tryToLogout(); return false;"" rel=""nofollow"" title=""Odjavite se iz sistema"">Odjavite se</a>
	</div>
	
</div>
<form action="""" method=""post"">
	<div id=""topline"" style=""background:#EC9C1E; height:92px; padding:5px 0;"">
		<div id=""logo"">
			<a href=""/"" title=""PolovniAutomobili.com - auto oglasi - prodaja novih i polovnih vozila""><img alt=""PolovniAutomobili.com - auto oglasi - prodaja novih i polovnih vozila"" src=""/images/polovniautomobili.com-logo-black.png"" /></a>
		</div>
		
		<div id=""main_banner"">
			<script type=""text/javascript"">
			// <![CDATA[
			    var ord=Math.round(Math.random()*100000000);
			    document.write('<sc'+'ript type=""text/javascript"" src=""http://ad.adverticum.net/js.prm?zona=7000558&ord='+ord+'""><\/scr'+'ipt>');
			// ]]>
			</script>
			<noscript><a href=""http://ad.adverticum.net/click.prm?zona=7000558&nah=!ie"" target=""_blank"" title=""Click on the advertisement!""><img border=""0"" src=""http://ad.adverticum.net/img.prm?zona=7000558&nah=!ie"" alt=""Advertisement"" /></a></noscript>
		</div>
		
		<div id=""logginbox"" style=""width:111px; float:right;"">

			<div id=""loginbox2"" style=""display: none;"">
				<div style=""width:100px"">
					<input type=""button"" class=""button"" value=""Prijava"" onclick=""tryToLoginFromHeader();"" />
					<label for=""remember"" class=""remember""><input type=""checkbox"" id=""rememberme_header"" class=""input_checkbox"" value=""yes"" />
						Zapamti me</label>
				</div>
				<div class=""links"">
					-<a href=""#"" onclick=""Modalbox.show($('lostPassOverlay')); return false;"" rel=""nofollow"">Zaboravljena šifra?</a><br>
					-<a href=""/registracija.php"" rel=""nofollow"" class=""register"">Registruj se!</a><br />
					<a href=""https://www.facebook.com/dialog/oauth?client_id=127593208027&redirect_uri=http%3A%2F%2Fwww.polovniautomobili.com%2Fmoj-profil.php%3Flogin%3Dfacebook%26fbloggedlink%3Dtrue&state=7d0749d462f92ef9ac5e89394f5d92b5&scope=email%2Cuser_location"">
					<img src=""/images/fblogin-button.png"" style=""height:24px; width:101px; margin:-1px 0 0 -3px;"" />
					</a>
				</div>
			</div>
			<div id=""logged_banner"" style=""display: block"">
                
			</div>
		</div>
		<div class=""clearboth""></div>
	</div>
</form>
<div class=""clearboth""></div>
<div id=""main-horiz-nav"">
	<div>
		<ul class=""megamenu"">
			<li><a href=""/"" title=""Naslovna stranica"" class="" selected"" style=""padding-top:4px; padding-bottom:2px;""><img src=""/images/home.png"" border=""0""></a></li>
			<li style=""background-image:url(/images/nova-vozila-linija.png);""><a class=""main "" href=""/putnicka-vozila-26.php?showold_pt=false&shownew_pt=true&showoldnew=new"" title=""Lista aktuelnih oglasa novih vozila"">
				Nova vozila
				<img src=""/images/mega-menu-arrow.png"" align=""absmiddle"" border=""0""></a>

				<div style=""width: 150px; left:43px"">
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-automobili"" title="""" class=""link"">Nova putnička vozila</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-motori"" title="""" class=""link"">Novi motori</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-kombi-i-laka-dostavna-vozila"" title="""" class=""link"">Novi kombiji i laka dostavna vozila</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-kamioni-do-7-5t"" title="""" class=""link"">Nivo kamioni do 7t</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/nove-prikolice-poluprikolice"" title="""" class=""link"">Nove prikolice i poluprikolice</a>
					</p>
					
					<p class=""line-h"" style="""">
						<a href=""/nova-poljoprivredna-vozila"" title="""" class=""link"">Nova poljoprivredna vozila</a>
					</p>
					
				</div>
			</li>
			<li><a class=""main "" href=""/auto-prodavci.php"" title=""Spisak prodavaca koji svoju ponudu predstavljaju na sajtu"">
				Prodavci
				<img src=""/images/mega-menu-arrow.png"" align=""absmiddle"" border=""0""></a>

				<div style=""width: 150px; left:143px"">
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/auto-dileri.php"" title="""" class=""link"">Dileri</a>
					</p>

					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/auto-saloni.php"" title="""" class=""link"">Saloni</a>
					</p>

					<p class=""line-h"">
						<a href=""/auto-lizing.php"" title="""" class=""link"">Lizing kuće</a>
					</p>
				</div>
			</li>
			<li><a href=""/auto-usluge.php"" title=""Baza auto usluga"" class="""">Auto usluge</a></li>
			<li><a class=""main "" href=""/auto-vesti/"" title=""Razne vesti iz auto-moto industrije"">Auto vesti
				<img src=""/images/mega-menu-arrow.png"" align=""absmiddle"" border=""0""></a>

				<div style=""width: 860px; left:207px"">
					<div style=""width:160px; float:left; margin-right:10px"">
						<a href=""/auto-vesti/saveti"" title=""Saveti"" class=""title shadow"">Saveti</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/kako-izabrati-polovni-automobil.php"" class=""link"">??k? izabrati polovni automobil - šta gla...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/video-saveti-za-kupovinu-polovnog-automobila-ii-deo.php"" class=""link"">Video saveti za kupovinu polovnog automobila...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/video-saveti-za-kupovinu-polovnog-automobila-i-deo.php"" class=""link"">Video saveti za kupovinu polovnog automobila...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/kako-odrediti-cenu-automobila-koji-prodajete.php"" class=""link"">Kako odrediti cenu automobila koji prodajete...</a></p>
							
						</div>
						<a href=""/auto-vesti/saveti"" title=""Saveti"" class=""link other"">Svi saveti &raquo;</a>
					</div>
					<div style=""width:160px; float:left;"">
						<a href=""/auto-vesti/aktuelno/"" title=""Aktuelno"" class=""title shadow"">Aktuelno</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/tv-emisija-abs-show-i-sajt-www-polovniautomobili-com.php"" class=""link"">Video izveštaji sa sajma automobila u Be...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/kia-rio-1-2-cvvt-vs-kia-rio-1-4-crdi.php"" class=""link"">Rio - crno na belo</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/saradnja-sajtova-polovniautomobili-com-i-subotica-com.php"" class=""link"">Saradnja sajtova polovniautomobili.com i...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/abs-show-251.php"" class=""link"">ABS Show 251</a></p>
							
						</div>
						<a href=""/auto-vesti/aktuelno/"" title=""Aktuelno"" class=""link other"">Sve aktuelnosti &raquo;</a>
					</div>
					<div style=""width:160px; float:left; margin-left:10px"">
						<a href=""/auto-vesti/zanimljivosti/"" title=""Zanimljivosti"" class=""title shadow"">Zanimljivosti</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/vw-grupa-krupnim-koracima-napred.php"" class=""link"">VW Grupa krupnim koracima napred</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/2016-audi-a-5-novi-detalji.php"" class=""link"">2016 Audi A5 novi detalji</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/zeleznice-oglasile-prodaju-103-automobila.php"" class=""link"">????????? ???????? ??????? 103 ?????????...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/blindirani-mercedes-m-klase.php"" class=""link"">Blindirani Mercedes M klase</a></p>
							
						</div>
						<a href=""/auto-vesti/zanimljivosti/"" title=""Zanimljivosti"" class=""link other"">Sve zanimljivosti &raquo;</a>
					</div>
					<div style=""width:160px; float:left; margin-left:10px"">
						<a href=""/auto-vesti/tuning/"" title=""Tuning"" class=""title shadow"">Tuning</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/folksvagen-xl1-hibrid-super-stedisa.php"" class=""link"">Folksvagen XL1"" - hibrid super štediša</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/novobeogradski-tuning-styling-show.php"" class=""link"">Novobeogradski Tuning Styling Show</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/kicherer-tjunirani-mercedes-benzsls-ammg.php"" class=""link"">Kicherer: tjunirani Mercedes-Benz SLS AM...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/kreativne-ideje-mercedes-benz-a.php"" class=""link"">Kreativne ideje “Mercedes Benz”-a</a></p>
							
						</div>
						<a href=""/auto-vesti/tuning/"" title=""Tuning"" class=""link other"">Sve o tuningu &raquo;</a>
					</div>
					<div style=""width:160px; float:left; margin-left:10px"">
						<a href=""/auto-vesti/noviteti/"" title=""Noviteti"" class=""title shadow"">Noviteti</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php"" class=""link"">Ogromna potražnja za Ford Focus ST, proddavaniji...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/predstavljen-renault-clio-gt.php"" class=""link"">Predstavljen Renault Clio GT</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/audi-a3-sportback-g-tron.php"" class=""link"">Audi A3 Sportback G-Tron</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/predstavljen-vw-golf-gtd.php"" class=""link"">Predstavljen VW Golf GTD</a></p>
							
						</div>
						<a href=""/auto-vesti/noviteti/"" title=""Noviteti"" class=""link other"">Svi noviteti &raquo;</a>
					</div>
					<div class=""cb""></div>
				</div>
			</li>
			<li>
				<a class=""main"" href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=link_meni_pa"" title=""Internet prodaja guma"" target=""_blank"">Kupite gume
					<img src=""/images/ipg-logo-mm-mini.png"" align=""absmiddle"" border=""0""></a>
				<div style=""width: 665px; left:233px; overflow:hidden;"">
						<div style=""text-align:center; background-color:#33383C; border-radius:5px; margin-bottom:5px;""><a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni"" target=""_blank""><img src=""/images/ipg-logo-mm.png""></a></div>
					<div style=""width:150px; float:left;"">
						<a href=""http://www.internet-prodaja-guma.com/gume-na-akciji/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_gume_na_akciji"" class=""title shadow"" target=""_blank"">Zimske gume na akciji</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[6]=zimska&extra_fields[9]=MICHELIN&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_michelin"" class=""link"" target=""_blank"">Michelin </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[6]=zimska&extra_fields[9]=BF+GOODRICH&extra_fields[14]=Putni%C4%8Dko+vozilo&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_bf_goodrich "" class=""link"" target=""_blank"">BF Goodrich </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[6]=zimska&extra_fields[9]=SAVA&extra_fields[14]=Putni%C4%8Dko+vozilo&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_sava"" class=""link"" target=""_blank"">Sava </a></p>
						</div>
					</div>
					<div style=""width:150px; float:left; margin-left:10px"">
						<a href=""http://www.internet-prodaja-guma.com/gume-na-akciji/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_gume_na_akciji"" class=""title shadow"" target=""_blank"">Letnje gume na akciji</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&by_title=Y&by_shortdescr=Y&by_fulldescr=Y&by_sku=Y&including=all&substring=cordiant+comfort+ps400&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_cordiant"" class=""link"" target=""_blank"">Cordiant</a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/zeetex?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_zeetex"" class=""link"" target=""_blank"">Zeetex</a></p>
						</p>
						</div>
					</div>

	<div style=""width:165px; float:left; margin-left:10px"">
						<a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_najprodavanije_dimenzije"" class=""title shadow"" target=""_blank"">Najprodavanije dimenzije</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&by_title=Y&by_shortdescr=Y&by_fulldescr=Y&by_sku=Y&including=all&substring=205%2F55+R16&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_205_55_r16"" class=""link"" target=""_blank"">205/55 R16</a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&by_title=Y&by_shortdescr=Y&by_fulldescr=Y&by_sku=Y&including=all&substring=195%2F65+R15&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_195_65_r15"" class=""link"" target=""_blank"">195/65 R15 </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[1]=225&extra_fields[2]=45&extra_fields[3]=17&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_225_45_r17"" class=""link"" target=""_blank"">225/45 R17 </a></p>
						</div>
					</div>

					<div style=""width:150px; float:left; margin-left:10px"">
						<a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_pogodnosti"" class=""title shadow"" target=""_blank"">Pogodnosti</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/pages.php?pageid=17&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_gume_za_firme"" class=""link"" target=""_blank"">Gume za firme</a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/pages.php?pageid=15&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_popust_pri_montazi"" class=""link"" target=""_blank"">Popusti pri montaži </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/help.php?section=contactus&mode=update&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_upit_za_gume"" class=""link"" target=""_blank"">Upit za gume </a></p>
						</div>
					</div>

				</div>
			</li>
			<li><a class=""main"" href=""http://www.mojagaraza.rs/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_mg"" target=""_blank"">Diskutujte na <img src=""/images/moja-garaza-icon.png"" width=""14"" align=""absmiddle"" border=""0""></a>
				<div style=""width: 640px; left:233px; overflow:hidden;"">

						<div style=""text-align:center; background-color:#33383C; border-radius:5px; margin-bottom:5px;""><a href=""http://www.mojagaraza.rs"" target=""_blank""><img src=""/images/moja-garaza-logo.png"" ></a></div>
					<div style=""width:200px; float:left;"">
						<a href=""http://www.mojagaraza.rs/#!filter-reviews"" class=""title shadow"" target=""_blank"">Ako planirate kupovinu</a>

						<div>
						<img src=""/images/icons/potrazi-pa.png"" style=""position:absolute; top:36px; left:180px;"">
						<p class=""line-h""><a href=""http://www.mojagaraza.rs/#!filter-reviews"" class=""link"" target=""_blank"">Pogledajte ocene i utiske drugih vozača poput vas.</a></p>
						</div>
					</div>
					<div style=""width:200px; float:left; margin-left:10px"">
						<a href=""http://www.mojagaraza.rs/#!filter-questions"" class=""title shadow"" target=""_blank"">Pitanja i saveti</a>

						<div>
						<img src=""/images/icons/pitanja-pa.png"" style=""position:absolute; top:36px; left:390px;"">
						<p class=""line-h""><a href=""http://www.mojagaraza.rs/#!filter-questions"" class=""link"" target=""_blank"">Ako imate kvar ili neki drugi problem postavite pitanje, majstori odgovaraju.</a></p>
						</p>
						</div>
					</div>
					<div style=""width:200px; float:left; margin-left:10px"">
						<a href=""http://www.mojagaraza.rs/#!filter-galleries"" class=""title shadow"" target=""_blank"">Fotogalerije</a>

						<div>
						<img src=""/images/icons/fotogalerije-pa.png"" style=""position:absolute; top:36px; left:600px;"">
						<p class=""line-h""><a href=""http://www.mojagaraza.rs/#!filter-galleries"" class=""link"" target=""_blank"">Kupili ste super automobil, pokažite ga. Proverite šta misle drugi o vašem vozilu.</a></p>
						</div>
					</div>

				</div>


			</li>

			<li><a href=""/oglas/novi.php"" title=""Oglasite prodaju vozila, mašina, plovila, bicikala, delova i opreme"" rel=""nofollow"" class="""">Postavite oglas</a>
			</li>
			<li>
				<a href=""/moj-profil.php"" title=""Pogledajte vaš profil - vozila koja trenutno oglašavate, listu isteklih oglasa..."" rel=""nofollow"" class="""">Moj profil</a>
			</li>
			<li style=""border-right:none"">
				<a href=""/ponuda-za-oglasavanje.php"" title=""Izaberite odgovarajući paket i predstavite vašu ponudu"" class="""">Ponuda za oglašavanje</a>
			</li>


		</ul>
	</div>
</div>


<!--[if lte IE 6]>
<div id=""ie6block"" style=""background-color:#FFF9D4; font-size: 12px; font-weight: bold; color:#666666; border:#FFCC00 1px solid; margin:20px;"">
	<table cellpadding=""5"" cellspacing=""5"" style=""background-color:#FFF9D4;"">
		<tr align=""left"" style=""background-color:#FFF9D4;"">
			<td style=""width:100%;background-color:#FFF9D4;"">Ovaj sajt ne podrĹľava Internet Explorer 6 ili starije
				verzije!<br />
				<br />
				Molim Vas da preuzmete neki od ponuÄ‘enih modernijih Internet pregledaÄŤa.<br>
				<br />
				ViĹˇe o ovome na sledeÄ‡oj stranici:
				<br />
				<a href=""/wiki/ukidamo_podrsku_za_internet_explorer_6.php"" target=""_blank"" onClick=""var date = new Date();date.setTime(date.getTime()+(24*60*60*1000));document.cookie = 'ie6block=1; expires=' + date.toGMTString() + '; path=/';"" class=""blue-link"">
					<strong>Uskoro ukidamo podrĹˇku za Internet Explorer 6</strong> </a></td>
			<td style=""width:60%;background-color:#FFF9D4;"">
				<table border='0' align='center' cellpadding=""3"" cellspacing=""0"" style=""border:#999999 1px solid; background-color: #fff;"">
					<tr align='center'>
						<td colspan=""4"">
							<div align=""left"">PodrĹľavani Internet pretraĹľivaÄŤi</div>
						</td>
					<tr align='center'>
						<td><a href='http://www.mozilla.com/sr/'> <img border='0' src='/images/ie6block/firefox.gif' />
						</a></td>
						<td><a href='http://www.google.com/chrome'> <img border='0' src='/images/ie6block/chrome.gif' />
						</a></td>
						<td><a href='http://www.opera.com/'> <img border='0' src='/images/ie6block/opera.gif' /> </a>
						</td>
						<td><a href='http://www.microsoft.com/windows/internet-explorer/default.aspx'>
							<img border='0' src='/images/ie6block/ie.gif' /> </a></td>
					<tr align='center'>
						<td>Firefox</td>
						<td>Google Chrome
						</td>
						<td>Opera</td>
						<td>Internet Explorer 8</td>
					</tr>
				</table>
			</td>
			<td valign=""top"" style=""background-color:#FFF9D4;"">
				<a href="""" onClick=""document.getElementById('ie6block').style.display = 'none'; var date = new Date();date.setTime(date.getTime()+(24*60*60*1000));document.cookie = 'ie6block=1; expires=' + date.toGMTString() + '; path=/'; return false;"">
					<div title=""IgnoriĹˇi upozorenje"" style=""width:20px; line-height:20px; text-align:center; color:#000000; font-weight:bold; background-color:#FFFFFF; border:#999999 1px solid; cursor:pointer"">
						X</div>
				</a></td>
		</tr>
	</table>
</div>
<![endif]-->


<div class=""clearboth""></div>
<div id=""content"" class="""">
	<div id=""page"">
		<a name=""searchbox""></a>

		<div id=""navigation"" class=""extend"">
			
			<div id=""mainnav"" class=""main"">
				<ul>
					<li style=""width:108px"" class=""selected"">
						<a href=""/"" class=""carselected"">Putnička vozila</a></li>
					<li style=""width:64px"" class="""">
						<a href=""/motori.php"" class=""moto"">Motori</a></li>
					<li style=""width:125px"" class="""">
						<a href=""/kombi-i-laka-dostavna-vozila.php"" id=""transport_menuitem"" class=""transport"">Transportna vozila</a>
					</li>
					<li style=""width:136px"" class="""">
						<a href=""/poljoprivredna-vozila.php"" class=""agro"">Poljoprivredna vozila</a></li>
					<li style=""width:105px"" class="""">
						<a href=""/radne-masine.php"" class=""working"">Radne mašine</a></li>
					<li style=""width:80px"" class="""">
						<a href=""/plovila.php"" class=""nauti"">Plovila</a></li>
					<li style=""width:111px"" class="""">
						<a href=""/delovi-oprema.php"" class=""parts"">Delovi i oprema</a></li>
					<li style=""width:59px"" class="""">
						<a href=""/bicikli.php"" class=""bike"">Bicikli</a></li>
				</ul>
			</div>
			<div class=""cb""></div>
			<div id=""transport_menu"" style=""display:none;"">
				<ul style=""margin: 0 0 0 10px"">
					<li class=""""><a href=""/kombi-i-laka-dostavna-vozila.php"" class=""kombi"">Kombi i laka dostavna vozila</a>
					</li>
					<li class="""">
						<a href=""/kamioni-do-7-5t.php"" class=""mali-kamioni"">Kamioni do 7.5t</a></li>
					<li class=""""><a href=""/kamioni-preko-7-5t.php"" class=""veliki-kamioni"">Kamioni preko 7.5t</a>
					</li>
					<li class=""""><a href=""/prikolice-poluprikolice.php"" class=""prikolice"">Prikolice i poluprikolice</a></li>
					<li class=""""><a href=""/autobusi.php"" class=""autobusi"">Autobusi</a>
					</li>
					<li class=""""><a href=""/kamperi.php"" class=""kamperi"">Kamperi</a>
					</li>
				</ul>
			</div>
			
			
		</div>
		
		<div id=""main_content"" class=""main_content ""
		>
		

<div id=""frame"" class=""extend"">

	<div id=""additional"">
		
		<div id=""searchbar"">
			<h2>Pretraživač</h2>
			
			<form id=""mainSearchForm"" onsubmit=""submitMainSearch()"" action="""" method=""get"">
				<input type=""hidden"" id=""categoryId"" name=""categoryId"" value=""26"" />
				<input type=""hidden"" id=""search-page"" name=""search-page"" value="""" />
				<input type=""hidden"" id=""tags"" name=""tags"" value="""" />
				<input type=""hidden"" id=""tagsMulti"" name=""tagsMulti"" value="""" />
				<input type=""hidden"" id=""showonlypicture_pt"" name=""showonlypicture_pt"" value=""0"" />
				<input type=""hidden"" id=""hidedamaged_pt"" name=""hidedamaged_pt"" value=""0"" />
				<input type=""hidden"" id=""shownoprice_pt"" name=""shownoprice_pt"" value=""1"" />
				
				<input type=""hidden"" id=""showuserads_pt"" name=""showuserads_pt"" value=""1"" />
				<input type=""hidden"" id=""showdealerads_pt"" name=""showdealerads_pt"" value=""1"" />
				<input type=""hidden"" id=""showlizingads_pt"" name=""showlizingads_pt"" value=""1"" />
				<input type=""hidden"" id=""showsalonads_pt"" name=""showsalonads_pt"" value=""1"" />
				
				<input type=""hidden"" id=""showold_pt"" name=""showold_pt"" value=""1"" />
				<input type=""hidden"" id=""shownew_pt"" name=""shownew_pt"" value=""1"" />
				
				<input type=""hidden"" id=""datelimit_pt"" name=""datelimit"" value=""0"" />
				<input type=""hidden"" id=""search-advertiser"" name=""search-advertiser"" value=""0"" />
				<div>
					
					<h3>Marka</h3>
					<select name=""brand"" id=""brand"" onchange=""updateModels_v2(false, true);"" data-max_chars=""16"">
						
										<option value=""0"" >Sve marke</option>
										
										<option value=""3707"" >AC</option>
										
										<option value=""75"" >Acura</option>
										
										<option value=""1469"" >Aleko</option>
										
										<option value=""77"" >Alfa Romeo</option>
										
										<option value=""3014"" >Aro</option>
										
										<option value=""38"" >Audi</option>
										
										<option value=""82"" >Austin</option>
										
										<option value=""85"" >Bentley</option>
										
										<option value=""37"" >BMW</option>
										
										<option value=""90"" >Buick</option>
										
										<option value=""92"" >Cadillac</option>
										
										<option value=""3217"" >Chery</option>
										
										<option value=""95"" >Chevrolet</option>
										
										<option value=""96"" >Chrysler</option>
										
										<option value=""97"" >Citroen</option>
										
										<option value=""100"" >Corvette</option>
										
										<option value=""101"" >Dacia</option>
										
										<option value=""102"" >Daewoo</option>
										
										<option value=""103"" >Daihatsu</option>
										
										<option value=""106"" >Dodge</option>
										
										<option value=""108"" >Ferrari</option>
										
										<option value=""109"" >Fiat</option>
										
										<option value=""473"" >Ford</option>
										
										<option value=""3000"" >GAZ</option>
										
										<option value=""110"" >GMC</option>
										
										<option value=""114"" >Honda</option>
										
										<option value=""115"" >Hummer</option>
										
										<option value=""116"" >Hyundai</option>
										
										<option value=""117"" >Infiniti</option>
										
										<option value=""119"" >Isuzu</option>
										
										<option value=""120"" >Jaguar</option>
										
										<option value=""121"" >Jeep</option>
										
										<option value=""3695"" >Katay Gonow</option>
										
										<option value=""123"" >Kia</option>
										
										<option value=""126"" >Lada</option>
										
										<option value=""128"" >Lancia</option>
										
										<option value=""129"" >Land Rover</option>
										
										<option value=""167"" >Lexus</option>
										
										<option value=""169"" >Lincoln</option>
										
										<option value=""170"" >Lotus</option>
										
										<option value=""171"" >Mahindra</option>
										
										<option value=""172"" >Maserati</option>
										
										<option value=""174"" >Mazda</option>
										
										<option value=""175"" >Mercedes Benz</option>
										
										<option value=""176"" >Mercury</option>
										
										<option value=""178"" >Mini</option>
										
										<option value=""177"" >MG</option>
										
										<option value=""179"" >Mitsubishi</option>
										
										<option value=""2339"" >Moszkvics</option>
										
										<option value=""181"" >Nissan</option>
										
										<option value=""184"" >Oldsmobile</option>
										
										<option value=""56"" >Opel</option>
										
										<option value=""185"" >Peugeot</option>
										
										<option value=""186"" >Piaggio</option>
										
										<option value=""2347"" >Polonez</option>
										
										<option value=""2340"" >Polski Fiat</option>
										
										<option value=""188"" >Pontiac</option>
										
										<option value=""189"" >Porsche</option>
										
										<option value=""190"" >Proton</option>
										
										<option value=""192"" >Renault</option>
										
										<option value=""194"" >Rover</option>
										
										<option value=""195"" >Saab</option>
										
										<option value=""197"" >Seat</option>
										
										<option value=""2991"" >Shuanghuan</option>
										
										<option value=""2342"" >Simca</option>
										
										<option value=""198"" >Smart</option>
										
										<option value=""200"" >SsangYong</option>
										
										<option value=""201"" >Subaru</option>
										
										<option value=""2099"" >Suzuki</option>
										
										<option value=""202"" >Škoda</option>
										
										<option value=""204"" >Tata</option>
										
										<option value=""3028"" >Tavria</option>
										
										<option value=""205"" >Toyota</option>
										
										<option value=""2773"" >Trabant</option>
										
										<option value=""2344"" >UAZ</option>
										
										<option value=""209"" >Vauxhall</option>
										
										<option value=""211"" >Volkswagen</option>
										
										<option value=""212"" >Volvo</option>
										
										<option value=""213"" >Wartburg</option>
										
										<option value=""2346"" >Yugo</option>
										
										<option value=""215"" >Zastava</option>
										
										<option value=""629"" >Ostalo</option>
										
					</select>
					<script>
						jQuery('#brand').multiselect({
							multiple		: false,
							minWidth		: 'auto',
							height			: 'auto',
							header			: false,
							noneSelectedText: 'Odaberi',
							selectedText	: multiselect_set_label,
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label
							});
					</script>
					
					
					<h3>Model</h3>
					<select name=""model"" id=""model"" disabled=""disabled"">
						
										<option value=""0"" >Svi modeli</option>
										
					</select>
					<script>
						jQuery('#model').multiselect({
							multiple		: false,
							minWidth		: 'auto',
							height			: 'auto',
							header			: false,
							noneSelectedText: 'Odaberi',
							selectedText	: multiselect_set_label,
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label,
							beforeopen		: multiselect_bold_options
							});
					</script>
					
					
					
						<h3>Obeležje</h3>
						<input name=""modeltxt"" id=""modeltxt"" type=""text"" class=""ui-corner-all ui-state-default"" value="""" />
					
					<h3>Cena</h3>
					<div class=""inlabel"">
						<label for=""price_from"">od</label>
						<input type=""text"" name=""price_from"" id=""price_from"" class=""ui-corner-all ui-state-default"" value="""" />
					</div>
					<div class=""inlabel"">
						<label for=""price_to"">do</label>
						<input type=""text"" name=""price_to"" id=""price_to"" class=""ui-corner-all ui-state-default"" value="""" />
					</div>
					
					<h3>Godina proizvodnje</h3>
					<div class=""inlabel"">
						<label for=""tag_218_from"">od</label>
						<select name=""tag_218_from"" id=""tag_218_from"" rel=""oddo_from"" >
							
										<option value=""0"" selected=""selected"">bilo koja</option>
										
										<option value=""2013.00"" >2013</option>
										
										<option value=""2012.00"" >2012</option>
										
										<option value=""2011.00"" >2011</option>
										
										<option value=""2010.00"" >2010</option>
										
										<option value=""2009.00"" >2009</option>
										
										<option value=""2008.00"" >2008</option>
										
										<option value=""2007.00"" >2007</option>
										
										<option value=""2006.00"" >2006</option>
										
										<option value=""2005.00"" >2005</option>
										
										<option value=""2004.00"" >2004</option>
										
										<option value=""2003.00"" >2003</option>
										
										<option value=""2002.00"" >2002</option>
										
										<option value=""2001.00"" >2001</option>
										
										<option value=""2000.00"" >2000</option>
										
										<option value=""1999.00"" >1999</option>
										
										<option value=""1998.00"" >1998</option>
										
										<option value=""1997.00"" >1997</option>
										
										<option value=""1996.00"" >1996</option>
										
										<option value=""1995.00"" >1995</option>
										
										<option value=""1994.00"" >1994</option>
										
										<option value=""1993.00"" >1993</option>
										
										<option value=""1992.00"" >1992</option>
										
										<option value=""1991.00"" >1991</option>
										
										<option value=""1990.00"" >1990</option>
										
										<option value=""1989.00"" >1989</option>
										
										<option value=""1988.00"" >1988</option>
										
										<option value=""1987.00"" >1987</option>
										
										<option value=""1986.00"" >1986</option>
										
										<option value=""1985.00"" >1985</option>
										
										<option value=""1984.00"" >1984</option>
										
										<option value=""1983.00"" >1983</option>
										
										<option value=""1982.00"" >1982</option>
										
										<option value=""1981.00"" >1981</option>
										
										<option value=""1980.00"" >1980</option>
										
										<option value=""1979.00"" >1979</option>
										
										<option value=""1978.00"" >1978</option>
										
										<option value=""1977.00"" >1977</option>
										
										<option value=""1976.00"" >1976</option>
										
										<option value=""1975.00"" >1975</option>
										
										<option value=""1970.00"" >1970</option>
										
										<option value=""1965.00"" >1965</option>
										
										<option value=""1960.00"" >1960</option>
										
										<option value=""1955.00"" >1955</option>
										
										<option value=""1950.00"" >1950</option>
										
										<option value=""1945.00"" >1945</option>
										
										<option value=""1940.00"" >1940</option>
										
										<option value=""1935.00"" >1935</option>
										
										<option value=""1930.00"" >1930</option>
										
						</select>
						<script>
							jQuery('#tag_218_from').multiselect({
								multiple		: false,
								minWidth		: 'auto',
								height			: 'auto',
								header			: false,
								noneSelectedText: 'Odaberi',
								selectedText	: multiselect_set_label,
								open			: multiselect_fit_widget,
								close			: multiselect_free_keyboard,
								create			: multiselect_fit_label
								});
						</script>
					</div>
					
					<div class=""inlabel"">
						<label for=""tag_218_to"">do</label>
						<select name=""tag_218_to"" id=""tag_218_to"" rel=""oddo_to"">
							
										<option value=""0"" selected=""selected"">bilo koja</option>
										
										<option value=""2013.00"" >2013</option>
										
										<option value=""2012.00"" >2012</option>
										
										<option value=""2011.00"" >2011</option>
										
										<option value=""2010.00"" >2010</option>
										
										<option value=""2009.00"" >2009</option>
										
										<option value=""2008.00"" >2008</option>
										
										<option value=""2007.00"" >2007</option>
										
										<option value=""2006.00"" >2006</option>
										
										<option value=""2005.00"" >2005</option>
										
										<option value=""2004.00"" >2004</option>
										
										<option value=""2003.00"" >2003</option>
										
										<option value=""2002.00"" >2002</option>
										
										<option value=""2001.00"" >2001</option>
										
										<option value=""2000.00"" >2000</option>
										
										<option value=""1999.00"" >1999</option>
										
										<option value=""1998.00"" >1998</option>
										
										<option value=""1997.00"" >1997</option>
										
										<option value=""1996.00"" >1996</option>
										
										<option value=""1995.00"" >1995</option>
										
										<option value=""1994.00"" >1994</option>
										
										<option value=""1993.00"" >1993</option>
										
										<option value=""1992.00"" >1992</option>
										
										<option value=""1991.00"" >1991</option>
										
										<option value=""1990.00"" >1990</option>
										
										<option value=""1989.00"" >1989</option>
										
										<option value=""1988.00"" >1988</option>
										
										<option value=""1987.00"" >1987</option>
										
										<option value=""1986.00"" >1986</option>
										
										<option value=""1985.00"" >1985</option>
										
										<option value=""1984.00"" >1984</option>
										
										<option value=""1983.00"" >1983</option>
										
										<option value=""1982.00"" >1982</option>
										
										<option value=""1981.00"" >1981</option>
										
										<option value=""1980.00"" >1980</option>
										
										<option value=""1979.00"" >1979</option>
										
										<option value=""1978.00"" >1978</option>
										
										<option value=""1977.00"" >1977</option>
										
										<option value=""1976.00"" >1976</option>
										
										<option value=""1975.00"" >1975</option>
										
										<option value=""1970.00"" >1970</option>
										
										<option value=""1965.00"" >1965</option>
										
										<option value=""1960.00"" >1960</option>
										
										<option value=""1955.00"" >1955</option>
										
										<option value=""1950.00"" >1950</option>
										
										<option value=""1945.00"" >1945</option>
										
										<option value=""1940.00"" >1940</option>
										
										<option value=""1935.00"" >1935</option>
										
										<option value=""1930.00"" >1930</option>
										
						</select>
						<script>
							jQuery('#tag_218_to').multiselect({
								multiple		: false,
								minWidth		: 'auto',
								height			: 'auto',
								header			: false,
								noneSelectedText: 'Odaberi',
								selectedText	: multiselect_set_label,
								open			: multiselect_fit_widget,
								close			: multiselect_free_keyboard,
								create			: multiselect_fit_label
								});
						</script>
					</div>
					
					
					
					
					<h3>Karoserija</h3>
					<select class=""multi_generic"" name=""tag_142[]"" id=""tag_142"" multiple=""multiple"" size=""5"" data-id=""142"">
					
										<option value=""277"" >Limuzina</option>
										
										<option value=""2631"" >Hečbek</option>
										
										<option value=""278"" >Karavan</option>
										
										<option value=""2633"" >Kupe</option>
										
										<option value=""2634"" >Kabriolet/Rodster</option>
										
										<option value=""2636"" >Monovolumen (MiniVan)</option>
										
										<option value=""2632"" >Džip/SUV</option>
										
										<option value=""2635"" >Pick up</option>
										
					</select>
					<script>
						jQuery('.multi_generic').multiselect({
							height			: 'auto',
							noneSelectedText: 'Odaberi',
							checkAllText	: 'Sve',
							uncheckAllText	: 'Poni\u0161ti',
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label,
							selectedText	: multiselect_set_label
							});
					</script>
					
					<h3>Gorivo</h3>
					<select class=""multi_generic"" name=""tag_36[]"" id=""tag_36"" multiple=""multiple"" size=""5"" data-id=""36"">
					
										<option value=""45"" >Benzin</option>
										
										<option value=""2309"" >Dizel</option>
										
										<option value=""2310"" >Benzin + Gas (TNG)</option>
										
										<option value=""2311"" >Metan CNG</option>
										
										<option value=""2312"" >Električni pogon</option>
										
										<option value=""2308"" >Hibridni pogon</option>
										
					</select>
					<script>
						jQuery('.multi_generic').multiselect({
							height			: 'auto',
							noneSelectedText: 'Odaberi',
							checkAllText	: 'Sve',
							uncheckAllText	: 'Poni\u0161ti',
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label,
							selectedText	: multiselect_set_label
							});
					</script>
					
					
					<h3>Region</h3>
					<select name=""selectedRegion[]"" id=""region"" multiple=""multiple"">
						<optgroup label=""Velike regije"">
						
							<option value=""Beograd"" >Beograd</option>
						
							<option value=""Centralna Srbija"" >Centralna Srbija</option>
						
							<option value=""Istočna Srbija"" >Istočna Srbija</option>
						
							<option value=""Južna Srbija"" >Južna Srbija</option>
						
							<option value=""Kosovo i Metohija"" >Kosovo i Metohija</option>
						
							<option value=""Vojvodina"" >Vojvodina</option>
						
							<option value=""Zapadna Srbija"" >Zapadna Srbija</option>
						
						</optgroup>
						<optgroup label=""Okruzi"">
						
							<option value=""2544"" >Beograd (uži)</option>
						
							<option value=""2543"" >Beograd (širi)</option>
						
							<option value=""2557"" >Borski</option>
						
							<option value=""2554"" >Braničevski</option>
						
							<option value=""2566"" >Jablanički</option>
						
							<option value=""2550"" >Južno-bački</option>
						
							<option value=""2548"" >Južno-banatski</option>
						
							<option value=""2553"" >Kolubarski</option>
						
							<option value=""2568"" >Kosovski</option>
						
							<option value=""3017"" >Kosovsko-pomoravski</option>
						
							<option value=""2571"" >Kosovsko-mitrovački</option>
						
							<option value=""2552"" >Mačvanski</option>
						
							<option value=""2560"" >Moravički</option>
						
							<option value=""2563"" >Nišavski</option>
						
							<option value=""2567"" >Pčinjski</option>
						
							<option value=""2569"" >Pećki</option>
						
							<option value=""2565"" >Pirotski</option>
						
							<option value=""2542"" >Podunavski</option>
						
							<option value=""2556"" >Pomoravski</option>
						
							<option value=""2570"" >Prizrenski</option>
						
							<option value=""2562"" >Rasinski</option>
						
							<option value=""2561"" >Raški</option>
						
							<option value=""2545"" >Severno-bački</option>
						
							<option value=""2547"" >Severno-banatski</option>
						
							<option value=""2546"" >Srednje-banatski</option>
						
							<option value=""2551"" >Sremski</option>
						
							<option value=""2555"" >Šumadijski</option>
						
							<option value=""2564"" >Toplički</option>
						
							<option value=""2558"" >Zaječarski</option>
						
							<option value=""2549"" >Zapadno-bački</option>
						
							<option value=""2559"" >Zlatiborski</option>
						
							<option value=""25"" >Inostranstvo</option>
						
						</optgroup>
					</select>
					<script>
						jQuery('#region').multiselect({
							height			: 'auto',
							noneSelectedText: 'Odaberi',
							checkAllText	: 'Sve',
							uncheckAllText	: 'Poni\u0161ti',
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label,
							selectedText	: multiselect_set_label,
							beforeopen		: multiselect_bold_options
							});
					</script>
					
					
					<h3>Država</h3>
					<select name=""country"" id=""country"" >
						
										<option value=""0"" >Sve države</option>
										
										<option value=""RS"" >Srbija</option>
										
										<option value=""ME"" >Crna Gora</option>
										
										<option value=""BA"" >Bosna i Hercegovina</option>
										
										<option value=""BE"" >Belgija</option>
										
										<option value=""FR"" >Francuska</option>
										
										<option value=""NL"" >Holandija</option>
										
										<option value=""IT"" >Italija</option>
										
										<option value=""DE"" >Nemačka</option>
										
										<option value=""RU"" >Rusija</option>
										
										<option value=""CH"" >Švajcarska</option>
										
										<option value=""GB"" >Velika Britanija</option>
										
										<option value=""AT"" >Austrija</option>
										
										<option value=""BG"" >Bugarska</option>
										
										<option value=""CZ"" >Češka</option>
										
										<option value=""DK"" >Danska</option>
										
										<option value=""HR"" >Hrvatska</option>
										
										<option value=""IE"" >Irska</option>
										
										<option value=""LT"" >Litvanija</option>
										
										<option value=""LU"" >Luksemburg</option>
										
										<option value=""HU"" >Mađarska</option>
										
										<option value=""MK"" >Makedonija</option>
										
										<option value=""PT"" >Portugal</option>
										
										<option value=""RO"" >Rumunija</option>
										
										<option value=""SK"" >Slovačka</option>
										
										<option value=""SI"" >Slovenija</option>
										
										<option value=""ES"" >Španija</option>
										
										<option value=""SE"" >Švedska</option>
										
										<option value=""XX"" >Ostale zemlje</option>
										
					</select>
					<script>
						jQuery('#country').multiselect({
							multiple		: false,
							minWidth		: 'auto',
							height			: 'auto',
							header			: false,
							noneSelectedText: 'Odaberi',
							selectedText	: multiselect_set_label,
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label
							});
					</script>
					
					
					<h3>Pogledaj</h3>
					<select id=""showoldnew"" name=""showoldnew"" data-max_chars=""16"">
						<option value=""all"">Polovna i nova vozila</option>
						<option value=""old"" >Samo polovna vozila</option>
						<option value=""new"" >Samo nova vozila</option>
					</select>
					<script>
						jQuery('#showoldnew').multiselect({
							multiple		: false,
							minWidth		: 'auto',
							height			: 'auto',
							header			: false,
							noneSelectedText: 'Odaberi',
							selectedText	: multiselect_set_label,
							open			: multiselect_fit_widget,
							close			: multiselect_free_keyboard,
							create			: multiselect_fit_label
							});
					</script>
					
					<input type=""submit"" name="""" id=""mainSearchSubmit"" value=""Traži"" onclick=""changeMainSearchLink();"" class=""button"" />
				</div>
			</form>
			
			
			
			<div class=""links"">
				<a href=""#"" onclick=""toggleSearch(); return false;"" id=""toggleSearchLink"">
                    <span id=""detailsSearchText"" style=""display: block;"">Detaljni pretraživač &gt;</span>
                    <span id=""detailsOpenSearchText"" style=""display: none;"">&lt; Jednostavni pretraživač</span>
                </a>
			</div>
			<div id=""bm_this"" style=""display:none"">Zapamti pretragu</div>
			<div style=""height:30px;"">
				<p id=""addBookmarkContainer"" style=""margin-top:10px;width:100px;float:left;margin-left:15px;"">
					<div id=""bm_ttip"" style=""display:none;float:left;margin-top:17px;""> <img src=""/images/icons/icon-help.png"" id=""itooltip1"" style=""margin:0px;"" title=""
<div>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(1); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-left:13px; padding-top:5px;'>Opcija 'Zapamti pretragu' omogućava da u vašem Internet pregledniku sačuvate podešavanja koja svakodnevno koristite prilikom pretrage oglasa. Potrebno je da: <br />
1. Podesite filtere u detaljnoj pretrazi i napravite pretragu oglasa <br />
2. Nakon toga kliknete na opciju 'Zapamti pretragu' i odredite gde će u vašem Internet pregledniku (Mozilla Firefox, Internet Explorer) podešavanja pretrage biti sačuvana (Bookmarks) <br />
3. Kada kliknete na link koji je sačuvan u zabeleškama (Bookmarks) vašeg Internet preglednika, otvoriće se stranica sa oglasima koji odgovaraju parametrima pretrage koje ste sačuvali </div></div>

"" onload=""jQuery('#itooltip1').tooltip({ predelay: '400', effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right',tipClass:'widetooltip'});"" /></div>
				</p>
			</div>
			

			
			<div id=""detailed_search"" style=""display: none;"">
					<h2>Detaljni pretraživač</h2>
					<div class=""set extend search_options"">
						<h3>Prikaži</h3>
						<ul style=""width:60%; float:left;"">
							<li style=""margin-left:6px;""><input type=""checkbox"" name=""showbottompics"" value=""1"" id=""showbottompics"" onclick=""ShowBottomPics(this.checked)"" /> <label for=""showbottompics"">Prikaži slike i na dnu stranice detaljnog opisa oglasa</label></li>
							<li style=""margin-left:6px;""><input type=""checkbox"" name=""showonlypicture"" value=""1"" id=""showonlypicture""  /> <label for=""showonlypicture"">Prikaži samo oglase sa slikom</label></li>
							
								<li style=""margin-left:6px;""><input type=""checkbox"" name=""hidedamaged"" value=""1"" id=""hidedamaged""  /> <label for=""hidedamaged"">Prikaži vozila bez oštećenja</label></li>
							
							<li style=""width:155px; float:left; display:block"">
							
								<h3>Oglasi postavljeni</h3>
								<select id=""datelimit"">
									
										<option value=""0"" ></option>
										
										<option value=""1"" >u poslednja 24 časa</option>
										
										<option value=""3"" >u poslednja 3 dana</option>
										
										<option value=""7"" >u poslednjih 7 dana</option>
										
										<option value=""15"" >u poslednjih 15 dana</option>
										
								</select>
								<script>
									// Make the options more user-friendly
									jQuery('#datelimit option[value=""0""]').text('bilo kad');

									jQuery('#datelimit').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							
							</li>
							<li style=""width:150px; float:left; margin-left:10px;"">
								&nbsp;
							</li>
						</ul>
						<ul style=""width:40%; float:left;"">
							<li><input type=""checkbox"" name=""shownoprice"" value=""1"" id=""shownoprice""  checked=""checked"" /> <label for=""shownoprice"">Prikaži i oglase bez cene</label></li>
							<li><div id=""cloud_prices""><input id=""tag_2691"" type=""checkbox"" value=""2691"" name=""tag_2691""  /> <label for=""tag_2691"">Zamena</label></div></li>
							
							<li><div id=""cloud_lizing""><input id=""tag_3683"" type=""checkbox"" value=""3683"" name=""tag_3683""  /> <label for=""tag_3683"">Lizing</label></div></li>
							
						</ul>
						<div style=""width:155px; float:left; "">
							<h3>Prikaži</h3>
							<select id=""showudads"" name=""showudads[]"" multiple=""multiple"" style=""width:170px;"">
								<option value=""user"" >oglase fizičkih lica</option>
								<option value=""dealer"" >oglase dilera</option>
								<option value=""salon"" >oglase salona</option>
								<option value=""lizing"" >oglase lizing kuća</option>
							</select>
							<script>
								jQuery('#showudads').multiselect({
									height			: 'auto',
									noneSelectedText: 'oglase svih ogla\u0161iva\u010Da',
									checkAllText	: 'Sve',
									uncheckAllText	: 'Poni\u0161ti',
									open			: multiselect_fit_widget,
									close			: multiselect_free_keyboard,
									create			: multiselect_fit_label,
									selectedText	: multiselect_set_label
									});
							</script>
						</div>
					</div>
					
					<div class=""set extend font11"" id=""cloud_20"">
						<h3>Dodatne informacije</h3>
						

							
						<div class='element1' style='width:310px;'>
							<div style='float:left; width:155px;'>
								<h3>Kubikaža (cm3)</h3>
								<div class='inlabel'>
									<label for=""tag_136_from"">od</label>
									<select class=""multi_from"" name=""tag_136_from"" id=""tag_136_from"" rel=""oddo_from"" data-max_chars=""12"">
										
										<option value=""0"" ></option>
										
										<option value=""500.00"" >500</option>
										
										<option value=""1150.00"" >1150</option>
										
										<option value=""1300.00"" >1300</option>
										
										<option value=""1600.00"" >1600</option>
										
										<option value=""1800.00"" >1800</option>
										
										<option value=""2000.00"" >2000</option>
										
										<option value=""2500.00"" >2500</option>
										
										<option value=""3000.00"" >3000</option>
										
										<option value=""3500.00"" >3500</option>
										
									</select>
									<script>
										// Make the options more user-friendly
										jQuery('.multi_from option[value=""0""]').text('bilo koja');
										jQuery('.multi_from option[value=""0.00""]').remove();
										
										jQuery('.multi_from').multiselect({
											multiple		: false,
											minWidth		: 'auto',
											height			: 'auto',
											header			: false,
											noneSelectedText: 'Odaberi',
											selectedText	: multiselect_set_label,
											open			: multiselect_fit_widget,
											close			: multiselect_free_keyboard,
											create			: multiselect_fit_label
											});
									</script>
								</div>
							</div>
							<div style='float:left; width:155px;'>
							<h3>&nbsp;</h3>
								<div class='inlabel'>
									<label for=""tag_136_to"">do</label>
									<select class=""multi_to"" name=""tag_136_to"" id=""tag_136_to"" rel=""oddo_to"" data-max_chars=""12"">
										
										<option value=""0"" ></option>
										
										<option value=""1150.00"" >1150</option>
										
										<option value=""1300.00"" >1300</option>
										
										<option value=""1600.00"" >1600</option>
										
										<option value=""1800.00"" >1800</option>
										
										<option value=""2000.00"" >2000</option>
										
										<option value=""2500.00"" >2500</option>
										
										<option value=""3000.00"" >3000</option>
										
										<option value=""3500.00"" >3500</option>
										
										<option value=""9999999999999.00"" >više od 3500</option>
										
									</select>
									<script>
										jQuery('.multi_to option[value=""0""]').text('bilo koja');
										jQuery('.multi_to').multiselect({
											multiple		: false,
											minWidth		: 'auto',
											height			: 'auto',
											header			: false,
											noneSelectedText: 'Odaberi',
											selectedText	: multiselect_set_label,
											open			: multiselect_fit_widget,
											close			: multiselect_free_keyboard,
											create			: multiselect_fit_label
											});
									</script>
								</div>
							</div>
						</div>
							
							
							
							
							
							
							
						

							
						<div class='element1' style='width:310px;'>
							<div style='float:left; width:155px;'>
								<h3>Snaga (kW)</h3>
								<div class='inlabel'>
									<label for=""tag_137_from"">od</label>
									<select class=""multi_from"" name=""tag_137_from"" id=""tag_137_from"" rel=""oddo_from"" data-max_chars=""12"">
										
										<option value=""0"" ></option>
										
										<option value=""25.00"" >25kW (34KS)</option>
										
										<option value=""35.00"" >35kW (48KS)</option>
										
										<option value=""44.00"" >44kW (60KS)</option>
										
										<option value=""55.00"" >55kW (75KS)</option>
										
										<option value=""66.00"" >66kW (90KS)</option>
										
										<option value=""74.00"" >74kW (101KS)</option>
										
										<option value=""85.00"" >85kW (116KS)</option>
										
										<option value=""96.00"" >96kW (131KS)</option>
										
										<option value=""110.00"" >110kW (150KS)</option>
										
										<option value=""147.00"" >147kW (200KS)</option>
										
										<option value=""184.00"" >184kW (250KS)</option>
										
										<option value=""222.00"" >222kW (302KS)</option>
										
										<option value=""262.00"" >262kW (356KS)</option>
										
										<option value=""294.00"" >294kW (402KS)</option>
										
										<option value=""333.00"" >333kW (453KS)</option>
										
									</select>
									<script>
										// Make the options more user-friendly
										jQuery('.multi_from option[value=""0""]').text('bilo koja');
										jQuery('.multi_from option[value=""0.00""]').remove();
										
										jQuery('.multi_from').multiselect({
											multiple		: false,
											minWidth		: 'auto',
											height			: 'auto',
											header			: false,
											noneSelectedText: 'Odaberi',
											selectedText	: multiselect_set_label,
											open			: multiselect_fit_widget,
											close			: multiselect_free_keyboard,
											create			: multiselect_fit_label
											});
									</script>
								</div>
							</div>
							<div style='float:left; width:155px;'>
							<h3>&nbsp;</h3>
								<div class='inlabel'>
									<label for=""tag_137_to"">do</label>
									<select class=""multi_to"" name=""tag_137_to"" id=""tag_137_to"" rel=""oddo_to"" data-max_chars=""12"">
										
										<option value=""0"" ></option>
										
										<option value=""35.00"" >35kW (48KS)</option>
										
										<option value=""44.00"" >44kW (60KS)</option>
										
										<option value=""55.00"" >55kW (75KS)</option>
										
										<option value=""66.00"" >66kW (90KS)</option>
										
										<option value=""74.00"" >74kW (101KS)</option>
										
										<option value=""85.00"" >85kW (116KS)</option>
										
										<option value=""96.00"" >96kW (131KS)</option>
										
										<option value=""110.00"" >110kW (150KS)</option>
										
										<option value=""147.00"" >147kW (200KS)</option>
										
										<option value=""184.00"" >184kW (250KS)</option>
										
										<option value=""222.00"" >222kW (302KS)</option>
										
										<option value=""262.00"" >262kW (356KS)</option>
										
										<option value=""294.00"" >294kW (402KS)</option>
										
										<option value=""333.00"" >333kW (453KS)</option>
										
										<option value=""999999999.00"" >više od 333kW (453KS)</option>
										
									</select>
									<script>
										jQuery('.multi_to option[value=""0""]').text('bilo koja');
										jQuery('.multi_to').multiselect({
											multiple		: false,
											minWidth		: 'auto',
											height			: 'auto',
											header			: false,
											noneSelectedText: 'Odaberi',
											selectedText	: multiselect_set_label,
											open			: multiselect_fit_widget,
											close			: multiselect_free_keyboard,
											create			: multiselect_fit_label
											});
									</script>
								</div>
							</div>
						</div>
							
							
							
							
							
							
							
						

							
						<div class='element1' style='width:310px;'>
							<div style='float:left; width:155px;'>
								<h3>Kilometraža</h3>
								<div class='inlabel'>
									<label for=""tag_131_from"">od</label>
									<select class=""multi_from"" name=""tag_131_from"" id=""tag_131_from"" rel=""oddo_from"" data-max_chars=""12"">
										
										<option value=""0"" ></option>
										
										<option value=""0.00"" >---</option>
										
										<option value=""25000.00"" >25000</option>
										
										<option value=""50000.00"" >50000</option>
										
										<option value=""75000.00"" >75000</option>
										
										<option value=""100000.00"" >100000</option>
										
										<option value=""125000.00"" >125000</option>
										
										<option value=""150000.00"" >150000</option>
										
										<option value=""200000.00"" >200000</option>
										
									</select>
									<script>
										// Make the options more user-friendly
										jQuery('.multi_from option[value=""0""]').text('bilo koja');
										jQuery('.multi_from option[value=""0.00""]').remove();
										
										jQuery('.multi_from').multiselect({
											multiple		: false,
											minWidth		: 'auto',
											height			: 'auto',
											header			: false,
											noneSelectedText: 'Odaberi',
											selectedText	: multiselect_set_label,
											open			: multiselect_fit_widget,
											close			: multiselect_free_keyboard,
											create			: multiselect_fit_label
											});
									</script>
								</div>
							</div>
							<div style='float:left; width:155px;'>
							<h3>&nbsp;</h3>
								<div class='inlabel'>
									<label for=""tag_131_to"">do</label>
									<select class=""multi_to"" name=""tag_131_to"" id=""tag_131_to"" rel=""oddo_to"" data-max_chars=""12"">
										
										<option value=""0"" ></option>
										
										<option value=""25000.00"" >25000</option>
										
										<option value=""50000.00"" >50000</option>
										
										<option value=""75000.00"" >75000</option>
										
										<option value=""100000.00"" >100000</option>
										
										<option value=""125000.00"" >125000</option>
										
										<option value=""150000.00"" >150000</option>
										
										<option value=""200000.00"" >200000</option>
										
										<option value=""999999999.00"" >250000 i više</option>
										
									</select>
									<script>
										jQuery('.multi_to option[value=""0""]').text('bilo koja');
										jQuery('.multi_to').multiselect({
											multiple		: false,
											minWidth		: 'auto',
											height			: 'auto',
											header			: false,
											noneSelectedText: 'Odaberi',
											selectedText	: multiselect_set_label,
											open			: multiselect_fit_widget,
											close			: multiselect_free_keyboard,
											create			: multiselect_fit_label
											});
									</script>
								</div>
							</div>
						</div>
							
							
							
							
							
							
							
						

							
							
							
							
							<div class='element1'>
							<h3>Emisiona klasa motora</h3>
								<select class=""multi_levels"" name=""tag_3252"" id=""tag_3252"" rel=""levels"">
								
										<option value=""0"" ></option>
										
										<option value=""2587"" >Euro 1</option>
										
										<option value=""2588"" >Euro 2</option>
										
										<option value=""2589"" >Euro 3</option>
										
										<option value=""2590"" >Euro 4</option>
										
										<option value=""2591"" >Euro 5</option>
										
								</select>
								<script>
									jQuery('.multi_levels option[value=""0""]').text('Odaberi');
									jQuery('.multi_levels').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							</div>
							
							
							
							
						

							
							
							
							<div class='element1'>
							<h3>Pogon </h3>
								<select class=""multi_dropdown"" name=""tag_144"" id=""tag_144"" rel=""dropdown"">
								
										<option value=""0"" ></option>
										
										<option value=""293"" >Prednji</option>
										
										<option value=""294"" >Zadnji</option>
										
										<option value=""295"" >4x4</option>
										
										<option value=""296"" >4x4 reduktor</option>
										
								</select>
								<script>
									jQuery('.multi_dropdown option[value=""0""]').text('Odaberi');
									jQuery('.multi_dropdown').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							</div>
							
							
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Menjač </h3>
								<select class=""multi_generic"" name=""tag_138[]"" id=""tag_138"" multiple=""multiple"" size=""5"" data-id=""138"">
								
										<option value=""3210"" >Manuelni 4 brzine</option>
										
										<option value=""3211"" >Manuelni 5 brzina</option>
										
										<option value=""3212"" >Manuelni 6 brzina</option>
										
										<option value=""250"" >Poluautomatski</option>
										
										<option value=""251"" >Automatski</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						

							
							
							
							<div class='element1'>
							<h3>Broj vrata </h3>
								<select class=""multi_dropdown"" name=""tag_140"" id=""tag_140"" rel=""dropdown"">
								
										<option value=""0"" ></option>
										
										<option value=""3012"" >2/3 vrata</option>
										
										<option value=""3013"" >4/5 vrata</option>
										
								</select>
								<script>
									jQuery('.multi_dropdown option[value=""0""]').text('Odaberi');
									jQuery('.multi_dropdown').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							</div>
							
							
							
							
							
						

							
							
							
							<div class='element1'>
							<h3>Broj sedišta </h3>
								<select class=""multi_dropdown"" name=""tag_2701"" id=""tag_2701"" rel=""dropdown"">
								
										<option value=""0"" ></option>
										
										<option value=""3193"" >2 sedišta</option>
										
										<option value=""3702"" >3 sedišta</option>
										
										<option value=""3194"" >4 sedišta</option>
										
										<option value=""3195"" >5 sedišta</option>
										
										<option value=""3196"" >6 sedišta</option>
										
										<option value=""3197"" >7 sedišta</option>
										
										<option value=""3198"" >8 sedišta</option>
										
										<option value=""3199"" >9 sedišta</option>
										
								</select>
								<script>
									jQuery('.multi_dropdown option[value=""0""]').text('Odaberi');
									jQuery('.multi_dropdown').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							</div>
							
							
							
							
							
						

							
							
							
							<div class='element1'>
							<h3>Strana volana </h3>
								<select class=""multi_dropdown"" name=""tag_2627"" id=""tag_2627"" rel=""dropdown"">
								
										<option value=""0"" ></option>
										
										<option value=""2630"" >Levi volan</option>
										
										<option value=""2289"" >Desni volan</option>
										
								</select>
								<script>
									jQuery('.multi_dropdown option[value=""0""]').text('Odaberi');
									jQuery('.multi_dropdown').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							</div>
							
							
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Klima </h3>
								<select class=""multi_generic"" name=""tag_60[]"" id=""tag_60"" multiple=""multiple"" size=""5"" data-id=""60"">
								
										<option value=""3159"" >Manuelna klima</option>
										
										<option value=""3160"" >Automatska klima</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Boja </h3>
								<select class=""multi_generic"" name=""tag_58[]"" id=""tag_58"" multiple=""multiple"" size=""5"" data-id=""58"">
								
										<option value=""2573"" >Bela</option>
										
										<option value=""253"" >Bež</option>
										
										<option value=""254"" >Bordo</option>
										
										<option value=""2574"" >Braon</option>
										
										<option value=""255"" >Crna</option>
										
										<option value=""59"" >Crvena</option>
										
										<option value=""2575"" >Kameleon</option>
										
										<option value=""3328"" >Krem</option>
										
										<option value=""256"" >Ljubičasta</option>
										
										<option value=""2578"" >Narandžasta</option>
										
										<option value=""57"" >Plava</option>
										
										<option value=""258"" >Siva</option>
										
										<option value=""259"" >Smeđa</option>
										
										<option value=""260"" >Srebrna</option>
										
										<option value=""2577"" >Tirkiz</option>
										
										<option value=""2576"" >Teget</option>
										
										<option value=""261"" >Zelena</option>
										
										<option value=""262"" >Zlatna</option>
										
										<option value=""263"" >Žuta</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Materijal enterijera </h3>
								<select class=""multi_generic"" name=""tag_3835[]"" id=""tag_3835"" multiple=""multiple"" size=""5"" data-id=""3835"">
								
										<option value=""3830"" >Štof</option>
										
										<option value=""3831"" >Prirodna koža</option>
										
										<option value=""3832"" >Kombinovana koža</option>
										
										<option value=""3833"" >Velur</option>
										
										<option value=""3834"" >Drugi</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Boja enterijera </h3>
								<select class=""multi_generic"" name=""tag_3841[]"" id=""tag_3841"" multiple=""multiple"" size=""5"" data-id=""3841"">
								
										<option value=""3836"" >Crna</option>
										
										<option value=""3837"" >Bež</option>
										
										<option value=""3838"" >Smeđa</option>
										
										<option value=""3839"" >Siva</option>
										
										<option value=""3840"" >Druga</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						

							
							
							
							
							<div class='element1'>
							<h3>Registrovan do</h3>
								<select class=""multi_levels"" name=""tag_139"" id=""tag_139"" rel=""levels"">
								
										<option value=""0"" ></option>
										
										<option value=""264"" >Nije registrovan</option>
										
										<option value=""3705"" >01.2013.</option>
										
										<option value=""3708"" >02.2013.</option>
										
										<option value=""3709"" >03.2013.</option>
										
										<option value=""3712"" >04.2013.</option>
										
										<option value=""3716"" >05.2013.</option>
										
										<option value=""3722"" >06.2013.</option>
										
										<option value=""3727"" >07.2013.</option>
										
										<option value=""3732"" >08.2013.</option>
										
										<option value=""3733"" >09.2013.</option>
										
										<option value=""3735"" >10.2013.</option>
										
										<option value=""3752"" >11.2013.</option>
										
										<option value=""3755"" >12.2013.</option>
										
										<option value=""3762"" >01.2014.</option>
										
										<option value=""3820"" >02.2014.</option>
										
										<option value=""3821"" >03.2014.</option>
										
								</select>
								<script>
									jQuery('.multi_levels option[value=""0""]').text('Odaberi');
									jQuery('.multi_levels').multiselect({
										multiple		: false,
										minWidth		: 'auto',
										height			: 'auto',
										header			: false,
										noneSelectedText: 'Odaberi',
										selectedText	: multiselect_set_label,
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label
										});
								</script>
							</div>
							
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Poreklo vozila <img src=""/images/icons/icon-help.png"" id=""itooltip3046"" style=""margin:0px;"" title=""
<div style='width:250px;''>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(3046); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-top:5px;'>
Izraz ’<strong>na ime kupca</strong>’ označava da kupcu ostaje da plati samo troškove registracije vozila. <br />Izraz ’<strong>stranac</strong>’ znači da vozilo ima strane tablice u odnosu za zemlju u kojoj se prodaje.<br /> Izraz ’<strong>domaće tablice</strong>’ znači da vozilo ima tablice zemlje u kojoj se prodaje.
</div></div>

"" onload=""jQuery('#itooltip3046').tooltip({ effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right'});"" /></h3>
								<select class=""multi_generic"" name=""tag_3046[]"" id=""tag_3046"" multiple=""multiple"" size=""5"" data-id=""3046"">
								
										<option value=""2687"" >Domaće tablice</option>
										
										<option value=""3051"" >Na ime kupca</option>
										
										<option value=""2272"" >Strane tablice</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						

							
							
							
							
							
							<div  class='element1'>
								<h3>Oštećenje </h3>
								<select class=""multi_generic"" name=""tag_3796[]"" id=""tag_3796"" multiple=""multiple"" size=""5"" data-id=""3796"">
								
										<option value=""3799"" >Nije oštećen</option>
										
										<option value=""3798"" >Oštećen - u voznom stanju</option>
										
										<option value=""3797"" >Oštećen - nije u voznom stanju</option>
										
								</select>
								<script>
									jQuery('.multi_generic').multiselect({
										height			: 'auto',
										noneSelectedText: 'Odaberi',
										checkAllText	: 'Sve',
										uncheckAllText	: 'Poni\u0161ti',
										open			: multiselect_fit_widget,
										close			: multiselect_free_keyboard,
										create			: multiselect_fit_label,
										selectedText	: multiselect_set_label
										});
								</script>
							</div>
							
							
							
						
					</div>
					
					<div class=""set extend font11"" id=""cloud_16"">
						<h3>Sigurnost</h3>
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2585"" value=""2585"" id=""tag_2585""  /> <label for=""tag_2585"">Airbag za vozača </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2584"" value=""2584"" id=""tag_2584""  /> <label for=""tag_2584"">Airbag za suvozača </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2583"" value=""2583"" id=""tag_2583""  /> <label for=""tag_2583"">Bočni airbag </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_71"" value=""71"" id=""tag_71""  /> <label for=""tag_71"">Child lock <img src=""/images/icons/icon-help.png"" id=""itooltip71"" style=""margin:0px;"" title=""
<div style='width:250px;''>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(71); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-top:5px;'>
Prekidač kojim se blokira otvaranje zadnjih vrata.
</div></div>

"" onload=""jQuery('#itooltip71').tooltip({ effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right'});"" />
</label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_54"" value=""54"" id=""tag_54""  /> <label for=""tag_54"">ABS <img src=""/images/icons/icon-help.png"" id=""itooltip54"" style=""margin:0px;"" title=""
<div style='width:250px;''>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(54); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-top:5px;'>
Sistem protiv blokiranja kočnica.
</div></div>

"" onload=""jQuery('#itooltip54').tooltip({ effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right'});"" />
</label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_55"" value=""55"" id=""tag_55""  /> <label for=""tag_55"">ESP <img src=""/images/icons/icon-help.png"" id=""itooltip55"" style=""margin:0px;"" title=""
<div style='width:250px;''>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(55); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-top:5px;'>
Elektronska kontrola stabilnosti.
</div></div>

"" onload=""jQuery('#itooltip55').tooltip({ effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right'});"" />
</label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_72"" value=""72"" id=""tag_72""  /> <label for=""tag_72"">ASR <img src=""/images/icons/icon-help.png"" id=""itooltip72"" style=""margin:0px;"" title=""
<div style='width:250px;''>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(72); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-top:5px;'>
Sistem kontrole proklizavanja pogonskih točkova (kontrola trakcije).
</div></div>

"" onload=""jQuery('#itooltip72').tooltip({ effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right'});"" />
</label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_69"" value=""69"" id=""tag_69""  /> <label for=""tag_69"">Alarm </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_70"" value=""70"" id=""tag_70""  /> <label for=""tag_70"">Kodiran ključ </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2294"" value=""2294"" id=""tag_2294""  /> <label for=""tag_2294"">Blokada motora </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_68"" value=""68"" id=""tag_68""  /> <label for=""tag_68"">Centralno zaključavanje </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2295"" value=""2295"" id=""tag_2295""  /> <label for=""tag_2295"">Zeder </label></div>
							
						
					</div>
					
					<div class=""set extend font11"" id=""cloud_86"">
						<h3>Oprema</h3>
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2249"" value=""2249"" id=""tag_2249""  /> <label for=""tag_2249"">Metalik boja </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2796"" value=""2796"" id=""tag_2796""  /> <label for=""tag_2796"">Branici u boji auta </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_152"" value=""152"" id=""tag_152""  /> <label for=""tag_152"">Servo volan </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2307"" value=""2307"" id=""tag_2307""  /> <label for=""tag_2307"">Multifunkcionalni volan </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_153"" value=""153"" id=""tag_153""  /> <label for=""tag_153"">Tempomat </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2297"" value=""2297"" id=""tag_2297""  /> <label for=""tag_2297"">Daljinsko zaključavanje </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2598"" value=""2598"" id=""tag_2598""  /> <label for=""tag_2598"">Putni računar </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2594"" value=""2594"" id=""tag_2594""  /> <label for=""tag_2594"">Šiber </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_3089"" value=""3089"" id=""tag_3089""  /> <label for=""tag_3089"">Panorama krov </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_156"" value=""156"" id=""tag_156""  /> <label for=""tag_156"">Tonirana stakla </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_147"" value=""147"" id=""tag_147""  /> <label for=""tag_147"">Električni prozori </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_154"" value=""154"" id=""tag_154""  /> <label for=""tag_154"">Električni retrovizori </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2306"" value=""2306"" id=""tag_2306""  /> <label for=""tag_2306"">Grejači retrovizora </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2599"" value=""2599"" id=""tag_2599""  /> <label for=""tag_2599"">Sedišta podesiva po visini </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2304"" value=""2304"" id=""tag_2304""  /> <label for=""tag_2304"">Elektro podesiva sedišta </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2595"" value=""2595"" id=""tag_2595""  /> <label for=""tag_2595"">Grejanje sedišta </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2592"" value=""2592"" id=""tag_2592""  /> <label for=""tag_2592"">Kožna sedišta </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2596"" value=""2596"" id=""tag_2596""  /> <label for=""tag_2596"">Svetla za maglu </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2617"" value=""2617"" id=""tag_2617""  /> <label for=""tag_2617"">Xenon svetla </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2788"" value=""2788"" id=""tag_2788""  /> <label for=""tag_2788"">Senzori za svetla </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2787"" value=""2787"" id=""tag_2787""  /> <label for=""tag_2787"">Senzori za kišu </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2593"" value=""2593"" id=""tag_2593""  /> <label for=""tag_2593"">Parking senzori </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_856"" value=""856"" id=""tag_856""  /> <label for=""tag_856"">Webasto </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_160"" value=""160"" id=""tag_160""  /> <label for=""tag_160"">Krovni nosač </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2789"" value=""2789"" id=""tag_2789""  /> <label for=""tag_2789"">Kuka za vuču </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2600"" value=""2600"" id=""tag_2600""  /> <label for=""tag_2600"">Aluminijumske felne </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_148"" value=""148"" id=""tag_148""  /> <label for=""tag_148"">Navigacija </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_3764"" value=""3764"" id=""tag_3764""  /> <label for=""tag_3764"">Bluetooth </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2300"" value=""2300"" id=""tag_2300""  /> <label for=""tag_2300"">Radio/Kasetofon </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2301"" value=""2301"" id=""tag_2301""  /> <label for=""tag_2301"">Radio CD </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2302"" value=""2302"" id=""tag_2302""  /> <label for=""tag_2302"">CD changer </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2303"" value=""2303"" id=""tag_2303""  /> <label for=""tag_2303"">DVD/TV </label></div>
							
						
					</div>
					
					<div class=""set extend font11"" id=""cloud_15"">
						<h3>Stanje vozila</h3>
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_52"" value=""52"" id=""tag_52""  /> <label for=""tag_52"">Prvi vlasnik <img src=""/images/icons/icon-help.png"" id=""itooltip52"" style=""margin:0px;"" title=""
<div>
<div style='float:left; font-weight:bold; color:#B0122B'>
<img src='/images/icons/icon-help-red.png'> Info</div>
<div style='float:right'>
<a href='' onclick='closeToolTip(52); return false;'><img src='/images/icons/icon-close.png'></a></div>

<div style='clear:both; padding-left:13px; padding-top:5px;'>Prvi vlasnik vozila od kad je kupljeno kao novo.</div></div>

"" onload=""jQuery('#itooltip52').tooltip({ predelay: '400', effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,-21],position:'bottom right'});"" />
</label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_3319"" value=""3319"" id=""tag_3319""  /> <label for=""tag_3319"">Kupljen nov u Srbiji </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_63"" value=""63"" id=""tag_63""  /> <label for=""tag_63"">Garancija </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_53"" value=""53"" id=""tag_53""  /> <label for=""tag_53"">Garažiran </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2581"" value=""2581"" id=""tag_2581""  /> <label for=""tag_2581"">Servisna knjiga </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_3800"" value=""3800"" id=""tag_3800""  /> <label for=""tag_3800"">Rezervni ključ </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_73"" value=""73"" id=""tag_73""  /> <label for=""tag_73"">Restauriran </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2122"" value=""2122"" id=""tag_2122""  /> <label for=""tag_2122"">Oldtimer </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_65"" value=""65"" id=""tag_65""  /> <label for=""tag_65"">Prilagođeno invalidima </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_64"" value=""64"" id=""tag_64""  /> <label for=""tag_64"">Taxi </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2582"" value=""2582"" id=""tag_2582""  /> <label for=""tag_2582"">Test vozilo </label></div>
							
						

							
							
							
							
							
							
							
							<div style=""float: left; width: 155px; ""><input type=""checkbox"" name=""tag_2245"" value=""2245"" id=""tag_2245""  /> <label for=""tag_2245"">Tuning </label></div>
							
						
					</div>
					
					<div class=""center"" style=""margin-bottom: 10px;"">
						<input type=""button"" class=""button"" onclick=""$('mainSearchSubmit').click();"" value=""Traži"" />
					</div>
					<div class=""close"">
						<a href=""#"" onclick=""toggleSearch(); return false;"">Zatvori <span>X</span></a>
					</div>
					<div class=""connect""></div>
				<div class=""corner tl""></div><div class=""corner tr""></div><div class=""corner bl""></div><div class=""corner br""></div>
			</div>
			

		</div>
		
		
	
		<div class=""information-2"" id=""historybox"">
		    <div class=""corner tl""></div><div class=""corner tr""></div><div class=""corner bl""></div><div class=""corner br""></div>
		    <h2>Poslednje pogledani oglasi</h2>
		    <div class=""content"">
			<ul id=""adsHistory"" class=""links"">
			       <li id=""historyItem3116564"" class=""historyitem""><div class=""historyitemlist""><a href=""/oglas3116564/smart_forfour_13/"">Smart ForFour 1.3...</a></div><div class=""historyitemicon""><img onclick=""togglePickedSearchList('itemhistory3116564star', 'on_3116564'); return false;"" onmouseover=""new Tip(this, 'Sačuvaj u izabrane oglase');"" id=""itemhistory3116564star""  src=""/images/button-star.gif""/></div></li>
			 </ul>
		    </div>
		</div>
	

		
		
		<div style=""padding:10px 20px; background-image:url(/images/reklama120x600.png); background-repeat:no-repeat; background-position:top;"">
			<div id=""zone7000621"" class=""goAdverticum""></div>
		</div>
		
		
		


<p>
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	

	

	

	

	

	

	

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	

	
	
	
	
	
	

	
	
	
	
	
	
	
	
		
	
	
	
		    
	
	
	
	
	
	
	
		
	
	
    


	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	

	

	

	

	

	

	

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	

	
	
	
	
	
	

	
	
	
	
	
	
	
	
		
	
	
	
		    
	
	
	
	
	    <div id=""featured_articles"" style=""background-color:#FFF;border: #fba60d 1px solid; border-radius:5px; -moz-border-radius:5px;"">
    
		<h2>Izdvojeni članci</h2>
		<ul class=""extend"">
		    <li class=""extend"">
			<a href=""/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php""><img style=""float:none;"" alt=""\""Hyundai\"" pustio u rad prvu u svetu liniju za serijsku proizvodnju automobila sa 0% štetne emisije"" src=""/portal/content/Zanimljivosti/201303/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku/thumb.jpg"" /></a>
			<strong><a href=""/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php"">&quot;Hyundai&quot; pustio u rad prvu u svetu liniju za serijsku proizvodnju automobila sa 0% štetne emisije</a></strong>
			<p>""Hyundai"" je zvanično pustio u rad prvu i jedinu u svetu liniju za serijsku proizvodnju automobila sa 0% štetne emisije gasova. ... <a href=""/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php"" rel=""nofollow"">Detaljnije &gt;</a></p>
			<div id=""share"">
			    <span>05.03.2013. | <a title=""Komentari"" href=""/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php#komentari"" class=""comment"">&nbsp;</a> |&nbsp; <a title=""Pošalji članak prijatelju"" href=""javascript:void(0);"" onclick=""sendToFriendWiki('hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku','/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php', '&quot;Hyundai&quot; pustio u rad prvu u svetu liniju za serijsku proizvodnju automobila sa 0% štetne emisije')"" class=""sendmail"">&nbsp;</a>| <a title=""Podeli sa prijateljima na Facebook-u"" href=""http://www.facebook.com/sharer.php?u=http://www.polovniautomobili.com/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php&t=&quot;Hyundai&quot; pustio u rad prvu u svetu liniju za serijsku proizvodnju automobila sa 0% štetne emisije"" class=""facebook""  onClick=""javascript:_gaq.push(['_trackPageview', '/facebook_share/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php']);"" target=""_blank"">&nbsp;</a>| <a title=""Podeli sa prijateljima na Twitter-u"" href=""http://twitter.com/home?status=&quot;Hyundai&quot; pustio u rad prvu u svetu liniju za serijsku proizvodnju automobila sa 0% štetne emisije: http://www.polovniautomobili.com/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php"" class=""twitter"" onClick=""javascript:_gaq.push(['_trackPageview', '/twitter_share/auto-vesti/zanimljivosti/hyundai-pustio-u-rad-prvu-u-svetu-liniju-za-serijsku.php']);"" target=""_blank"">&nbsp;</a>| Ocena: 4.9</span>
			</div>
			 <div style=""border-bottom: #DDD 1px solid;padding-top:10px;""></div>

			
		    </li>
		    <li class=""extend"">
			<a href=""/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php""><img style=""float:none;"" alt=""Ogromna potražnja za Ford Focus ST, prodavaniji od Golfa GTI"" src=""/portal/content/Noviteti/201303/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf/thumb.jpg"" /></a>
			<strong><a href=""/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php"">Ogromna potražnja za Ford Focus ST, prodavaniji od Golfa GTI</a></strong>
			<p>Multi-talentovani Ford Focus ST je u prodaji u Evropi od oktobra 2012. godine. To je daleko praktičnija petovratna alternativa od trovratnih hothatch kompaktnih opcija od oko 250 ks na tržištu. Nje... <a href=""/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php"" rel=""nofollow"">Detaljnije &gt;</a></p>
			<div id=""share"">
			    <span>13.03.2013. | <a title=""Komentari"" href=""/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php#komentari"" class=""comment"">&nbsp;</a> |&nbsp; <a title=""Pošalji članak prijatelju"" href=""javascript:void(0);"" onclick=""sendToFriendWiki('ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf','/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php', 'Ogromna potražnja za Ford Focus ST, prodavaniji od Golfa GTI')"" class=""sendmail"">&nbsp;</a>| <a title=""Podeli sa prijateljima na Facebook-u"" href=""http://www.facebook.com/sharer.php?u=http://www.polovniautomobili.com/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php&t=Ogromna potražnja za Ford Focus ST, prodavaniji od Golfa GTI"" class=""facebook""  onClick=""javascript:_gaq.push(['_trackPageview', '/facebook_share/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php']);"" target=""_blank"">&nbsp;</a>| <a title=""Podeli sa prijateljima na Twitter-u"" href=""http://twitter.com/home?status=Ogromna potražnja za Ford Focus ST, prodavaniji od Golfa GTI: http://www.polovniautomobili.com/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php"" class=""twitter"" onClick=""javascript:_gaq.push(['_trackPageview', '/twitter_share/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php']);"" target=""_blank"">&nbsp;</a>| Ocena: 3.7</span>
			</div>
			 <div style=""border-bottom: #DDD 1px solid;padding-top:10px;""></div>

			
		    </li>
		    
		</ul>

	    </div>
	
	
	
	
		
	
	
    


	</p>


		
	</div>


	
	
	
	<div id=""results"" class=""page_content"">
		<div class=""corner tl""></div><div class=""corner tr""></div><div class=""corner bl""></div><div class=""corner br""></div>
		<div class=""info""><strong>Kriterijumi pretraživanja:</strong> <span id=""search_string"">Putnička vozila</span></div>
		<input type=""hidden"" name=""search-page"" id=""search-page"" value="""" />
		<input type=""hidden"" name=""advertiser"" id=""search-advertiser"" value="""" />
		<div id=""upper-pagination"">
<div class=""options"">
	
	<div class=""counter"">
		Prikazano od 1 do 20 rezultata od ukupno <strong><span id=""numOfAds"">46961.</span></strong>
	</div>
	
	
	
	<div class=""order"">
		Sortiraj po
		
		<select id=""orderListSort"" onchange=""window.location = '/putnicka-vozila-26/'+this.value+'.php?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all'"">
			
			<option value=""renewDate_desc"" selected=""selected"">datumu objave silazno</option>
			
			<option value=""renewDate_asc"" >datumu objave uzlazno</option>
			
			<option value=""price_asc"" >ceni uzlazno</option>
			
			<option value=""price_desc"" >ceni silazno</option>
			
			<option value=""tagValue218_asc"" >godini proizvodnje uzlazno</option>
			
			<option value=""tagValue218_desc"" >godini proizvodnje silazno</option>
			
			<option value=""tagValue131_asc"" >kilometraži uzlazno</option>
			
			<option value=""tagValue131_desc"" >kilometraži silazno</option>
			
			<option value=""title_asc"" >naslovu uzlazno</option>
			
			<option value=""title_desc"" >naslovu silazno</option>
			
		</select>
	</div>
	


	

	
		
	<div class=""comparenoticetxt"">Za poređenje više oglasa kliknite na ikonicu</div>
	<div class=""comparenoticeimg""><img src=/images/button-add.gif width=""17"" height=""17"" border=""0""></div>
	
	
	
	<div class=""pages"" style=""float:left;"">
	<div style='height:1px;'></div>
		<ul>
			
			<li>1</li>
			<li><a href=""/putnicka-vozila-26/1/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">2</a></li>
				<li><a href=""/putnicka-vozila-26/2/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">3</a></li>
				<li>... <a href=""/putnicka-vozila-26/2348/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">2349</a></li>
			<li class=""next""><a href=""/putnicka-vozila-26/1/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">Sledeća &raquo;</a></li>
			
		</ul>
	</div>
	
	
	<div class=""pages"">
	<div style='height:1px;'></div>
		<ul>
		
		</ul>
	</div>
	
	
	
</div>
</div>
		
		<div class=""content"" id=""searchlist-items"">
			<ul class=""extend"">
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3116564/smart_forfour_13/"">Smart ForFour 1.3 </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;98.250 km</div>
							<div class=""titleyr"">&nbsp;2005 god.</div>
							<div class=""titlepr"">
								&nbsp;2.850 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3116564"" data-params='{""id"":""3116564"",""title"":""Smart ForFour 1.3"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3116564/smart_forfour_13/""><img alt=""Smart ForFour 1.3"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-120x90.jpg"" /></a>
						<div class=""image-count"">
						8 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/lk-team""><img src=""/user-images/superadvertiser/244661/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Benzin, 70kW (95KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Garažiran, Grejači retrovizora, ESP<br />
							
							Južno-bački okrug, 21000 Novi Sad<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3191938/citroen_saxo_11_klima/"">Citroen Saxo 1.1 klima </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;110.000 km</div>
							<div class=""titleyr"">&nbsp;2002 god.</div>
							<div class=""titlepr"">
								&nbsp;2.040 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3191938"" data-params='{""id"":""3191938"",""title"":""Citroen Saxo 1.1 klima"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3191938/citroen_saxo_11_klima/""><img alt=""Citroen Saxo 1.1 klima"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/319/3191938/4a0c1ff98648-120x90.jpg"" /></a>
						<div class=""image-count"">
						8 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/favoritplus""><img src=""/user-images/superadvertiser/76402/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Benzin, 44kW (60KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Putni računar, Child lock<br />
							
							Beograd (uži), 11000 Beograd - Voždovac<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class="""">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas2794529/mercedes_benz_s_300/"">Mercedes Benz S 300 </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;300.000 km</div>
							<div class=""titleyr"">&nbsp;1992 god.</div>
							<div class=""titlepr"">
								&nbsp;1.200 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_2794529"" data-params='{""id"":""2794529"",""title"":""Mercedes Benz S 300"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas2794529/mercedes_benz_s_300/""><img alt=""Mercedes Benz S 300"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/279/2794529/0f3304e84488-120x90.jpg"" /></a>
						<div class=""image-count"">
						3 slike
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
						<span>
							
							Limuzina<br />Crna, 4/5 vrata, 5 sedišta<br />
							
							Benzin + Gas (TNG), 100kW (136KS), Automatski menjač<br />
							
							Taxi<br />
							
							Beograd (širi), 11500 Obrenovac<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class="""">Klima</span>
							<span class="""">Desni volan</span>
							<span class=""selected"">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class="""">ABS</span>
						</div>
					
					</div>
				</li>
				
				<div style=""text-align:center;""><img src=""/images/reklama300x250.png"" alt=""Reklama"" border=""0""></div>
				<div id=""zone7000726"" class=""goAdverticum""></div>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3042425/opel_zafira_vidi_opis/"">Opel Zafira vidi opis </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;170.000 km</div>
							<div class=""titleyr"">&nbsp;2004 god.</div>
							<div class=""titlepr"">
								&nbsp;4.500 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3042425"" data-params='{""id"":""3042425"",""title"":""Opel Zafira vidi opis"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3042425/opel_zafira_vidi_opis/""><img alt=""Opel Zafira vidi opis"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/304/3042425/4437e828f0c3-120x90.jpg"" /></a>
						<div class=""image-count"">
						9 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
						<span>
							
							Monovolumen (MiniVan)<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Metan CNG, 71kW (97KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Krovni nosač, Child lock<br />
							
							Južno-banatski okrug, 26000 Pančevo<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3194231/audi_a4_19_tdi_130_ks/"">Audi A4 1.9 tdi  130 ks </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;172.000 km</div>
							<div class=""titleyr"">&nbsp;2003 god.</div>
							<div class=""titlepr"">
								&nbsp;5.500 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3194231"" data-params='{""id"":""3194231"",""title"":""Audi A4 1.9 tdi  130 ks"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3194231/audi_a4_19_tdi_130_ks/""><img alt=""Audi A4 1.9 tdi  130 ks"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/319/3194231/3b4845a8945b-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/spcarteam""><img src=""/user-images/superadvertiser/84650/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Karavan<br />Plava metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 96kW (131KS), Manuelni 5 brzina<br />
							
							Automatska klima, Servisna knjiga, Krovni nosač, ASR<br />
							
							Južno-banatski okrug, 26000 Pančevo<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3194206/rover_214/"">Rover 214 </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;160.000 km</div>
							<div class=""titleyr"">&nbsp;1997 god.</div>
							<div class=""titlepr"">
								&nbsp;800 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3194206"" data-params='{""id"":""3194206"",""title"":""Rover 214"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3194206/rover_214/""><img alt=""Rover 214"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/nophoto_120x90.gif"" /></a>
						<div class=""image-count"">
						 
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
						<span>
							
							Hečbek<br />Plava, 2/3 vrata, 5 sedišta<br />
							
							Benzin + Gas (TNG), 76kW (103KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Radio CD, Alarm<br />
							
							Beograd (uži), 11000 Beograd<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class=""selected"">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas2879940/fiat_doblo_benzin_metan/"">Fiat Doblo benzin .metan </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;126.000 km</div>
							<div class=""titleyr"">&nbsp;2003 god.</div>
							<div class=""titlepr"">
								&nbsp;3.200 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_2879940"" data-params='{""id"":""2879940"",""title"":""Fiat Doblo benzin .metan"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas2879940/fiat_doblo_benzin_metan/""><img alt=""Fiat Doblo benzin .metan"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/287/2879940/33a5d5cdc029-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/np-car""><img src=""/user-images/superadvertiser/355695/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Monovolumen (MiniVan)<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Benzin, 48kW (65KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Servisna knjiga, Krovni nosač, Alarm<br />
							
							Južno-banatski okrug, 26000 Pančevo<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas2860714/seat_ibiza_tdi/"">Seat Ibiza tdi </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;138.400 km</div>
							<div class=""titleyr"">&nbsp;2003 god.</div>
							<div class=""titlepr"">
								&nbsp;3.300 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_2860714"" data-params='{""id"":""2860714"",""title"":""Seat Ibiza tdi"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas2860714/seat_ibiza_tdi/""><img alt=""Seat Ibiza tdi"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/286/2860714/5985e14deee2-120x90.jpg"" /></a>
						<div class=""image-count"">
						11 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/np-car""><img src=""/user-images/superadvertiser/355695/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 55kW (75KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Servisna knjiga, Grejači retrovizora, ASR<br />
							
							Južno-banatski okrug, 26000 Pančevo<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3152489/peugeot_308_16_hdi_navigacija/"">Peugeot 308 1.6 hdi navigacija </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;155.809 km</div>
							<div class=""titleyr"">&nbsp;2009 god.</div>
							<div class=""titlepr"">
								&nbsp;7.790 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3152489"" data-params='{""id"":""3152489"",""title"":""Peugeot 308 1.6 hdi navigacija"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3152489/peugeot_308_16_hdi_navigacija/""><img alt=""Peugeot 308 1.6 hdi navigacija"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/315/3152489/99a755b31399-120x90.jpg"" /></a>
						<div class=""image-count"">
						11 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/newbalkan""><img src=""/user-images/superadvertiser/14678/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 66kW (90KS), Manuelni 5 brzina<br />
							
							Automatska klima, Servisna knjiga, Navigacija, ASR<br />
							
							Beograd (uži), 11070 Beograd - Novi Beograd<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3152224/opel_insignia_20_cdti/"">Opel Insignia 2.0 cdti </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;127.371 km</div>
							<div class=""titleyr"">&nbsp;2010 god.</div>
							<div class=""titlepr"">
								&nbsp;11.990 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3152224"" data-params='{""id"":""3152224"",""title"":""Opel Insignia 2.0 cdti"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3152224/opel_insignia_20_cdti/""><img alt=""Opel Insignia 2.0 cdti"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/315/3152224/519c520c528f-120x90.jpg"" /></a>
						<div class=""image-count"">
						9 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/newbalkan""><img src=""/user-images/superadvertiser/14678/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Crna metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 81kW (110KS), Manuelni 6 brzina<br />
							
							Automatska klima, Kuka za vuču, Alarm<br />
							
							Beograd (uži), 11070 Beograd - Novi Beograd<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3194134/skoda_fabia_12htp/"">Škoda Fabia 1.2htp </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;150.888 km</div>
							<div class=""titleyr"">&nbsp;2004 god.</div>
							<div class=""titlepr"">
								&nbsp;3.100 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3194134"" data-params='{""id"":""3194134"",""title"":""\u0160koda Fabia 1.2htp"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3194134/skoda_fabia_12htp/""><img alt=""Škoda Fabia 1.2htp"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/319/3194134/b2c0c2d80bfd-120x90.jpg"" /></a>
						<div class=""image-count"">
						10 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
						<span>
							
							Hečbek<br />Siva metalik, 4/5 vrata, 4 sedišta<br />
							
							Benzin + Gas (TNG), 55kW (75KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Kupljen nov u Srbiji, CD changer, Child lock<br />
							
							Beograd (uži), 11000 Stari Grad<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3172727/mercedes_benz_a_170_cdi/"">Mercedes Benz A 170 cdi </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;145.600 km</div>
							<div class=""titleyr"">&nbsp;2000 god.</div>
							<div class=""titlepr"">
								&nbsp;2.599 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3172727"" data-params='{""id"":""3172727"",""title"":""Mercedes Benz A 170 cdi"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3172727/mercedes_benz_a_170_cdi/""><img alt=""Mercedes Benz A 170 cdi"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/317/3172727/c004b98f3be9-120x90.jpg"" /></a>
						<div class=""image-count"">
						10 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/unix-sped""><img src=""/user-images/superadvertiser/234532/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Plava metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 66kW (90KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Garažiran, Šiber, ASR<br />
							
							Beograd (uži), 11040 Beograd-dedinje<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas2988096/mercedes_benz_b_180_cdi/"">Mercedes Benz B 180 cdi </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;138.302 km</div>
							<div class=""titleyr"">&nbsp;2007 god.</div>
							<div class=""titlepr"">
								&nbsp;9.890 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_2988096"" data-params='{""id"":""2988096"",""title"":""Mercedes Benz B 180 cdi"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas2988096/mercedes_benz_b_180_cdi/""><img alt=""Mercedes Benz B 180 cdi"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/298/2988096/67dbe607b866-120x90.jpg"" /></a>
						<div class=""image-count"">
						10 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/newbalkan""><img src=""/user-images/superadvertiser/14678/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Limuzina<br />Plava metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 80kW (109KS), Manuelni 6 brzina<br />
							
							Manuelna klima, Senzori za kišu, Alarm<br />
							
							Beograd (uži), 11070 Beograd - Novi Beograd<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3049908/mercedes_benz_c_200_kompresor/"">Mercedes Benz C 200 kompresor </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;141.000 km</div>
							<div class=""titleyr"">&nbsp;2003 god.</div>
							<div class=""titlepr"">
								&nbsp;5.500 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3049908"" data-params='{""id"":""3049908"",""title"":""Mercedes Benz C 200 kompresor"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3049908/mercedes_benz_c_200_kompresor/""><img alt=""Mercedes Benz C 200 kompresor"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/304/3049908/bf1c44203bbf-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/np-car""><img src=""/user-images/superadvertiser/355695/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Kupe<br />Siva metalik, 2/3 vrata, 5 sedišta<br />
							
							Benzin, 120kW (163KS), Manuelni 6 brzina<br />
							
							Automatska klima, Servisna knjiga, Kožna sedišta, ASR<br />
							
							Južno-banatski okrug, 26000 Pančevo<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3152390/ford_focus_18_tdci/"">Ford Focus 1.8 tdci </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;141.397 km</div>
							<div class=""titleyr"">&nbsp;2007 god.</div>
							<div class=""titlepr"">
								&nbsp;4.690 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3152390"" data-params='{""id"":""3152390"",""title"":""Ford Focus 1.8 tdci"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3152390/ford_focus_18_tdci/""><img alt=""Ford Focus 1.8 tdci"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/315/3152390/38f1704a42fc-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/newbalkan""><img src=""/user-images/superadvertiser/14678/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Karavan<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 85kW (116KS), Manuelni 5 brzina<br />
							
							Manuelna klima, Senzori za svetla, Child lock<br />
							
							Beograd (uži), 11070 Beograd - Novi Beograd<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3193496/toyota_avensis_20_d4d_110000_km/"">Toyota Avensis 2.0 d4d 110000 km </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;110.000 km</div>
							<div class=""titleyr"">&nbsp;2004 god.</div>
							<div class=""titlepr"">
								&nbsp;5.550 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3193496"" data-params='{""id"":""3193496"",""title"":""Toyota Avensis 2.0 d4d 110000 km"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3193496/toyota_avensis_20_d4d_110000_km/""><img alt=""Toyota Avensis 2.0 d4d 110000 km"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/319/3193496/ceccc0024909-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
						<span>
							
							Karavan<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 85kW (116KS), Manuelni 5 brzina<br />
							
							Automatska klima, Prvi vlasnik, CD changer, ASR<br />
							
							Beograd (širi), 11000 Beograd<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3102620/lancia_ypsilon_13multijet/"">Lancia Ypsilon 1.3multijet </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;120.000 km</div>
							<div class=""titleyr"">&nbsp;2008 god.</div>
							<div class=""titlepr"">
								&nbsp;5.350 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3102620"" data-params='{""id"":""3102620"",""title"":""Lancia Ypsilon 1.3multijet"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3102620/lancia_ypsilon_13multijet/""><img alt=""Lancia Ypsilon 1.3multijet"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/310/3102620/f97f124a4e6c-120x90.jpg"" /></a>
						<div class=""image-count"">
						10 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/lk-team""><img src=""/user-images/superadvertiser/244661/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Kupe<br />Siva metalik, 2/3 vrata, 5 sedišta<br />
							
							Dizel, 55kW (75KS), Automatski menjač<br />
							
							Manuelna klima, Garažiran, Grejači retrovizora, ESP<br />
							
							Južno-bački okrug, 21000 Novi Sad<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3193994/bmw_320_d/"">BMW 320 d </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;208.358 km</div>
							<div class=""titleyr"">&nbsp;2000 god.</div>
							<div class=""titlepr"">
								&nbsp;3.700 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3193994"" data-params='{""id"":""3193994"",""title"":""BMW 320 d"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3193994/bmw_320_d/""><img alt=""BMW 320 d"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/319/3193994/c7d6bfaf9a4e-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/mp-auto""><img src=""/user-images/superadvertiser/26046/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Karavan<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 100kW (136KS), Manuelni 5 brzina<br />
							
							Automatska klima, Garažiran, CD changer, ASR<br />
							
							Beograd (uži), 11070 Novi Beograd - Ledine<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3190154/renault_laguna_19_dci_full/"">Renault Laguna 1.9 dci full </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;181.232 km</div>
							<div class=""titleyr"">&nbsp;2003 god.</div>
							<div class=""titlepr"">
								&nbsp;2.999 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3190154"" data-params='{""id"":""3190154"",""title"":""Renault Laguna 1.9 dci full"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3190154/renault_laguna_19_dci_full/""><img alt=""Renault Laguna 1.9 dci full"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/319/3190154/a83209fa8042-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
						<span>
							
							Karavan<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 88kW (120KS), Manuelni 6 brzina<br />
							
							Automatska klima, Parking senzori, Child lock<br />
							
							Podunavski okrug, 11300 Smederevo<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class="""">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
				
				
				
				
				<li class=""item extend featured"">
					<div class=""box-title-lightorange title"" style='margin:0px 4px; '>
						<div class=""itemtitle""><a href=""/oglas3110451/hyundai_tucson_20_crdi/"">Hyundai Tucson 2.0 crdi </a></div>
						
						
						<div class=""title-additional"" id=""itooltip"" title="""">
							<div class=""titlekm"">&nbsp;153.000 km</div>
							<div class=""titleyr"">&nbsp;2005 god.</div>
							<div class=""titlepr"">
								&nbsp;6.700 &euro;
							</div>
							
						</div>
						
						<div class=""clearboth""></div>
					</div>
					<div class=""option_list"">
						<ul>
							
							<li>
								<a title=""Uporedi oglas sa drugim oglasima"" class=""toggle_compare "" id=""compare_3110451"" data-params='{""id"":""3110451"",""title"":""Hyundai Tucson 2.0 crdi"",""category"":""26""}' onclick=""compare.toggle(this);""></a>
							</li>
							
						</ul>
					</div>
					<div class=""image"">
						<a href=""/oglas3110451/hyundai_tucson_20_crdi/""><img alt=""Hyundai Tucson 2.0 crdi"" width=""120"" height=""90"" src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3110451/a68d44f07477-120x90.jpg"" /></a>
						<div class=""image-count"">
						12 slika
						</div>
					</div>
					<div class=""details"">
					<div style=""min-height:95px"">
					
					<div class=""salon"" align=""center"">
						<div style=""margin:3px 0 3px 0"">
							<a href=""/jpcompany""><img src=""/user-images/superadvertiser/55513/logo-small.jpg"" /></a>
						</div>
					</div>
					
						<span>
							
							Džip/SUV<br />Siva metalik, 4/5 vrata, 5 sedišta<br />
							
							Dizel, 92kW (125KS), Automatski menjač<br />
							
							Automatska klima, Krovni nosač, ASR<br />
							
							Južno-bački okrug, 21000 Novi Sad<br />
							
							
							<span>Datum objave: </span>
							<span>18.03.2013.</span>
							
						</span>
					</div>						
					
						<div class=""specials"">
							<span class="""">Novo</span>
							<span class=""selected"">Garancija</span>
							<span class="""">Lizing</span>
							<span class=""selected"">Klima</span>
							<span class="""">Desni volan</span>
							<span class="""">Oštećen</span>
							<span class="""">Strane tablice</span>
							<span class=""selected"">ABS</span>
						</div>
					
					</div>
				</li>
				
				
				
			</ul>
		</div>
		
		
		<div id=""lower-pagination"">
<div class=""options"">
	
	
	


	
	<div style=""float:right; padding-right:2px;"">
		Idi na stranicu
		<input type=""text"" name=""ipage"" id=""ipage"" size=""3"" maxlength=""4"" onkeydown=""iPageJump(event)"" style=""border:#FBE0B7 1px solid;"" />
		<img src=""/images/button-page.png"" id=""pagejumpimage"" onclick=""window.location = '/putnicka-vozila-26/'+PageJump(this)+'/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all'"" style=""float:none;"" align=""absmiddle"" />
	</div>
	

	
		
	<div class=""comparenoticetxt"">Za poređenje više oglasa kliknite na ikonicu</div>
	<div class=""comparenoticeimg""><img src=/images/button-add.gif width=""17"" height=""17"" border=""0""></div>
	
	
	
	<div class=""pages"" style=""float:left;"">
	<div style='height:1px;'></div>
		<ul>
			
			<li>1</li>
			<li><a href=""/putnicka-vozila-26/1/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">2</a></li>
				<li><a href=""/putnicka-vozila-26/2/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">3</a></li>
				<li>... <a href=""/putnicka-vozila-26/2348/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">2349</a></li>
			<li class=""next""><a href=""/putnicka-vozila-26/1/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&showoldnew=all"" onclick="""">Sledeća &raquo;</a></li>
			
		</ul>
	</div>
	
	
	<div class=""pages"">
	<div style='height:1px;'></div>
		<ul>
		
		</ul>
	</div>
	
	
	
</div>
</div>
	<div>
		<div class=""dealer-1""></div> <div class=""dealer-2""></div> <div class=""dealer-1-txt"">Polovna vozila</div>
		<div class=""auto-saloon-1""></div> <div class=""auto-saloon-2""></div> <div class=""auto-saloon-1-txt"">Nova vozila</div>
	</div>
	</div>
	
</div>


			</div>
		</div>
	</div>
	
	<div id=""banner"">
		<div class=""banner"">
			
			
			
				<div id=""information"" class=""information"">
					<div><h2><a style=""background-position:left; width:130px"" href=""/arhiva-novosti/"">Novosti na sajtu</a></h2></div>
					<div id=""top"">
						


<p>
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	

	

	

	

	

	

	

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	

	
	
	
	
	
	

	
	
	
	
	
	
	
	
		
	
	
	
		    
	
	
	
	
	
	
	
		
	
	
    


	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	

	

	

	

	

	

	

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	

	
	
	
	
	
	

	
	
	
	
	
	
	
	
		
	
	
	
		    
	
	
	
	
	
	
	
	
		<div id=""featured_articles"">
			<ul class=""extend jcarousel-skin-tango"" id=""novosti-carousel"" >
				
				<li class=""extend"">
					<h3><a href=""/auto-vesti/aktuelno/tv-emisija-abs-show-i-sajt-www-polovniautomobili-com.php"">Video izveštaji sa sajma automobila u Beogradu</a></h3>
					<p>Tv emisija ABS SHOW i sajt www.polovniautomobili.com za Vas će u toku sajma automobila u Beogradu, svakodnevno, počevši od 26.03.2013. pripremati video izveštaje sa promocija novih automobila .Na ...</p>
					<span>18.03.2013. | <a title=""Komentari"" href=""/auto-vesti/aktuelno/tv-emisija-abs-show-i-sajt-www-polovniautomobili-com.php#komentari"" class=""comment"">0</a></span>
				</li>
				
				<li class=""extend"">
					<h3><a href=""/auto-vesti/novosti/dve-nove-opcije-na-sajtu-materijal-enterijera-i-boja.php"">Dve nove opcije na sajtu - materijal enterijera i boja enterijera</a></h3>
					<p>Danas smo uveli dve nove opcije na naš sajt. 
Naime, u kategoriji ""putnička vozila"" uveli smo opcije materijal enterijera i boja enterijera koje za cilj imaju da posetiocima i oglašivačima obezbe...</p>
					<span>15.03.2013. | <a title=""Komentari"" href=""/auto-vesti/novosti/dve-nove-opcije-na-sajtu-materijal-enterijera-i-boja.php#komentari"" class=""comment"">2</a></span>
				</li>
				
				<li class=""extend"">
					<h3><a href=""/auto-vesti/aktuelno/saradnja-sajtova-polovniautomobili-com-i-subotica-com.php"">Saradnja sajtova polovniautomobili.com i subotica.com</a></h3>
					<p>Sajt www.polovniautomobili.com i poznati lokalni sajt www.subotica.com ostvarili su partnersku saradnju. ...</p>
					<span>04.03.2013. | <a title=""Komentari"" href=""/auto-vesti/aktuelno/saradnja-sajtova-polovniautomobili-com-i-subotica-com.php#komentari"" class=""comment"">0</a></span>
				</li>
				
			</ul>
			<script>
			jQuery('#novosti-carousel').jcarousel({vertical: true, visible:1, scroll:1});
			</script>
			
			<div class=""archive-link"" style=""margin-left:100px"">
				[<a href=""/arhiva-novosti/"">Arhiva</a>]
			</div>
			
		</div>
		
	
	
    


	</p>


					</div>
				</div>
			
			
			
			
			<div id=""ssbblock"" align=""center"" style=""padding:10px 0 10px 0; background-image:url(/images/reklama120x600.png); background-repeat:no-repeat; background-position:top;"">
				<div id=""zone7000635"" class=""goAdverticum""></div>
			</div>
			
		</div>
		
		
		
		
			<div id=""sitestats"" style=""width:154px; margin:20px auto;"">
				<div style=""text-align:right;"">Broj aktivnih oglasa: </div>
				<div style=""background-image:url(/images/tablica.png); background-repeat:no-repeat; text-align:center;  width:130px; font-size:22px; padding-left:24px; font-weight:bold; padding-top:5px; padding-bottom:5px;"">92 416</div>

				<div style=""text-align:right;"">Poseta u februaru: </div>
				<div style=""background-image:url(/images/tablica-yellow.png); background-repeat:no-repeat; text-align:center;  width:130px; font-size:22px; padding-left:24px; font-weight:bold; padding-top:5px; padding-bottom:5px;"">6 663 990</div>

			<div style=""text-align:center; color:#999; font-size:11px;"">(izvor: Google Analytics)</div>
			</div>
		
		
			<style type=""text/css"">
				.backtotop-floater {
					border:2px solid #EC9C1F;
					position: fixed;
					bottom: 165px;
					height: 18px;
					left: 50%;
					margin-left: 350px;
					padding:10px 25px;
					width: 88px;
					z-index: 9999;
					background-color: rgb(236, 236, 236);
					opacity: 0;
					filter:alpha(opacity=0);
					-moz-opacity:0;
					display: block;
				}
				.topsearch-floater {
					border:2px solid #EC9C1F;
					position: fixed;
					bottom: 210px;
					height: 18px;
					left: 50%;
					margin-left: 350px;
					padding:10px 10px 10px 16px;
					width: 112px;
					z-index: 9999;
					background-color: rgb(236, 236, 236);
					opacity: 0;
					filter:alpha(opacity=0);
					-moz-opacity:0;
					display: block;
				}
			</style>
			<div id=""topsearch"" class=""box-title-lightorange topsearch-floater"">
  				<a href="""" onclick=""return scrollAndSearch();"">Detaljni pretraživač</a>
  			</div>
			<div id=""topper"" class=""box-title-lightorange backtotop-floater"">
				<a href="""" onclick=""jQuery(window).scrollTop(0); return false;"">Povratak na vrh</a>
			</div>
			<script type=""text/javascript"">
			jQuery(window).scroll(function () {
				var dummy = jQuery('#sitestats').offset().top+150;
				var scrollpos = jQuery(window).height()+jQuery(window).scrollTop();
				if(scrollpos >= dummy)
					{
					tmp = scrollpos - dummy;
					perc = 100/dummy*tmp;
					if (jQuery('#categoryId').val() != 35 && jQuery('#detailsSearchText').length > 0)
						{
						jQuery('#topsearch').fadeTo(0, perc/100);
						}
					jQuery('#topper').fadeTo(0, perc/100);
					}
				else
					{
					jQuery('#topsearch').hide();
					jQuery('#topper').hide();
					}
				});
			</script>
		

	</div>
	
	<div style=""clear: both;""></div>
	<div id=""info"" class=""font11"">
		<p>Copyright &copy; <a href=""http://www.infostud.com"" title=""Infostud - grupa korisnih sajtova"" target=""_blank""><img src=""/images/infostud_logo.png"" alt=""Infostud - grupa korisnih sajtova""  border=""0"" align=""absbottom"" style=""margin: 0 5px -5px 5px""></a> 2000 - 2013.</p> 
		<p><ul>
	<li><a href=""http://www.internet-prodaja-guma.com"" target=""_blank"">internet-prodaja-guma.com</a></li>
	<li><a href=""http://www.mojagaraza.rs/"" target=""_blank"">mojagaraza.rs</a></li>
	<li><a href=""http://poslovi.infostud.com"" target=""_blank"">poslovi.infostud.com</a></li>
	<li><a href=""http://www.mojtim.com"" target=""_blank"">mojtim.com</a></li>
	<li><a href=""http://www.kursevi.com"" target=""_blank"">kursevi.com</a></li>
	<li><a href=""http://www.najstudent.com"" target=""_blank"">najstudent.com</a></li>
	<li><a href=""http://prijemni.infostud.com"" target=""_blank"">prijemni.infostud.com</a></li>
	<li style=""border-right:none""><a href=""http://www.putovanja.info"" target=""_blank"">putovanja.info</a></li>
</ul>
<p>Sadržaj sajta <a href=""http://www.polovniautomobili.com"" style=""color:#b72633"">polovniautomobili.com</a> je vlasništvo <a href=""http://www.infostud.com"" target=""_blank"" style=""color:#46a1e8"">Infostuda</a>. Zabranjeno je njegovo preuzimanje bez dozvole Infostuda, zarad komercijalne upotrebe ili u druge svrhe, osim za lične potrebe posetilaca sajta. <a href=""/wiki/uslovi-koriscenja.php"" style=""font-size:11px; font-weight:normal; color:#46a1e8"">Uslovi korišćenja</a></p></p>
		<p class=""isgroup"">Infostud je deo <a href=""http://www.almamedia.com/"" target=""_blank"" title=""Alma Media"">ALMA</a> grupacije.</p>
	</div>
</div>



<div id=""compare_box"" style=""display:none"" data-category=""26"" data-txt_add=""Uporedi oglas sa drugim oglasima"" data-txt_remove=""Ukloni iz poređenja"">
	<h2>Uporedi oglase</h2>
	<ul id=""compare_list"">
		<li id=""compare_item_template"" style=""display:none"">
			<div class=""link"">
				<a href=""xxxURLxxx"" rel=""nofollow"">xxxTITLExxx</a>
			</div>
			<div class=""option"">
				<a title=""Ukloni vozilo iz uporednog prikaza"" data-params='{}' onclick=""compare.remove(this);"" style=""cursor: pointer;"">
					<img alt="""" src=""/images/close.gif"" />
				</a>
			</div>
		</li>
	</ul>
	<div class=""compare_link"">
		<a href=""/compare_classifieds.php?categoryId=26"" rel=""nofollow"" title=""Uporedite izabrana vozila"">[ UPOREDI ]</a>
		&nbsp;&nbsp;
		<a title=""Zatvorite ovaj prozor. Obratite pažnju da će izabrana vozila biti trajno sklonjena sa ove liste. Za ponovno poređenje biće neophodno da još jednom izaberete vozilo."" onclick=""compare.remove_all();"" style=""cursor: pointer;"">[ ZATVORI ]</a>
	</div>
</div>














	
<div id=""android_banner"" style=""position:fixed;right:0;bottom:0; z-index:999999;"">
	<script type='text/javascript' src='http://partner.googleadservices.com/gampad/google_service.js'>
	</script>
	<script type='text/javascript'>
	GS_googleAddAdSenseService(""ca-pub-2661848902415312"");
	GS_googleEnableAllServices();
	</script>
	<script type='text/javascript'>
	GA_googleAddSlot(""ca-pub-2661848902415312"", ""pa_android_280x126"");
	</script>
	<script type='text/javascript'>
	GA_googleFetchAds();
	</script>
	<script type='text/javascript'>
	   GA_googleFillSlot(""pa_android_280x126"");
	   // Kill the banner after 30sec
	window.setTimeout(
		function() {
			document.getElementById('android_banner').style.display = 'none';
		},
		30000
	);
	</script>
</div>

<!--<script>
	jQuery(window).load(function () {
		var data = {};
		data.url = window.location.href;
		if (jQuery('body').data('ip'))
			{
			data.ip = jQuery('body').data('ip');
			}
		data.load_time = new Date().getTime() - PageTimerStart;

		jQuery.ajax({ url: ""https://api.mongolab.com/api/1/databases/polovni/collections/timing?apiKey=50a8ce09e4b0c737b232abe7"",
			data: JSON.stringify(data),
			type: ""POST"",
			contentType: ""application/json""
			});
		});
</script>-->

<!-- The g3.js should be called once in every page, before the end </body> tag -->
<script type=""text/javascript"" charset=""utf-8"" src=""//ad.adverticum.net/g3.js""></script>
</body>
</html>";
        }
        private string TestOglas()
        {
            return @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" lang=""SR"" xml:lang=""SR"">
<head>
	<title>2005 Smart ForFour 1.3 | Polovni automobili</title>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
	<meta name=""keywords"" content=""Smart ForFour 1.3, polovni Smart ForFour 1.3, prodaja polovnih automobila delova opreme, polovni automobili delovi oprema, polovni automobili prodaja, vozila, automobili, auto oglasi, auto delovi, vozilo, kola, automobil, kombi, motori, kamioni, polovni automobili beograd, automobili kupovina, polovni automobili kredit, polovni automobili iz uvoza"" />
	<meta name=""description"" content=""Smart ForFour 1.3 – Pronađite pravo vozilo za sebe na najposećenijem sajtu o automobilima i ostalim vozilima u Srbiji."" />
	<meta http-equiv=""X-UA-Compatible"" content=""IE=9"" />
	<meta http-equiv=""content-language"" content=""SR"">
	<meta name=""google-site-verification"" content=""9-7CPBCpKIMIbGvlGecaVh4sOTiPHORKoCzrywaFb88"" />
	<meta name=""alexaVerifyID"" content=""BlqQz4Lk4Lu0a3xuQEBI9g6jB5o"" />
	<meta property=""fb:page_id"" content=""36224102230"" />
	<meta property=""fb:app_id"" content=""127593208027""/>
	<meta property=""og:image"" content=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-400x300.jpg"" />
	<meta property=""og:title"" content=""2005 Smart ForFour 1.3 | Polovni automobili"" />
	<meta property=""og:type"" content=""website"" />
	<meta property=""og:url"" content=""http://www.polovniautomobili.com/oglas3116564/smart_forfour_13/"" />
	<meta property=""og:site_name"" content=""www.polovniautomobili.com"" />

	
	
	<script	type=""text/javascript"">
	var	PageTimerStart = new Date().getTime();
	</script>


	<link type=""text/css"" rel=""stylesheet"" media=""all"" href=""http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/themes/ui-lightness/jquery-ui.css"" />
	<link type=""text/css"" rel=""stylesheet"" media=""all"" href=""/css/jquery-ui-newpa/jquery-ui-1.10.0.custom.min.css"" />
	<script type=""text/javascript"" src=""//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js""></script>
	<script type=""text/javascript"" src=""//ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js""></script>
	
	<link type=""text/css"" rel=""stylesheet"" media=""screen"" href=""/css/pa.php"" />
	
	<link type=""text/css"" rel=""stylesheet"" media=""screen"" href=""/css/modalbox.php"" />
	
	<link type=""text/css"" rel=""stylesheet"" media=""screen"" href=""/css/jqui.php"" />
	
	<link type=""text/css"" rel=""stylesheet"" media=""print"" href=""/css/print.php"" />
	
	<script type=""text/javascript"" src=""/javascript.php?""></script>
	
	<script type=""text/javascript"" language=""javascript"" src=""//maps.googleapis.com/maps/api/js?sensor=false&key=AIzaSyCcjZnQjA2vrGbb9WcD8pe0SaKZVq81TJM""></script>
	
	<!-- GA code / -->

	<script type=""text/javascript"">
		var _gaq = _gaq || [];
		_gaq.push(['_setAccount', 'UA-220728-1']);
		_gaq.push(['_trackPageview']);

		(function()
			{
			var ga = document.createElement('script');
			ga.type = 'text/javascript';
			ga.async = true;
			ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
			var s = document.getElementsByTagName('script')[0];
			s.parentNode.insertBefore(ga, s);
			})();
	</script>

	<script type=""text/javascript"">
		jQuery(document).ready(function()
		{
		jQuery("".megamenu"").megamenu({
			'show_method':'simple',
			'hide_method': 'fadeOut',
			'enable_js_shadow':false
		});
		});
	</script>

	
	
	<script type=""text/javascript"">
	var banner_check = setInterval(function() {
		//console.log('checking banner..');
		if( jQuery('div[id^=""httpool_hlpContainer_""]').css('width') == '600px' && jQuery('div[id^=""httpool_hlpContainer_""]').css('height') == '250px' && jQuery('div[id^=""httpool_hlpContainer_""]').css('zIndex') != 'auto' ) {
			jQuery('div[id^=""httpool_hlpContainer_""]').css('zIndex', 'auto');
			//console.log('banner set');
			clearInterval(banner_check);
		}
	}, 1000);
	</script>
		
	<script type=""text/javascript"">
	var banner_check2 = setInterval(function() {
		//console.log('checking banner..');
		if( jQuery('div[id$=""_exp.inside_out""]').css('width') == '728px' && jQuery('div[id$=""_exp.inside_out""]').css('height') == '90px' && jQuery('div[id$=""_exp.inside_out""]').css('zIndex') != 24 ) {
			jQuery('div[id$=""_exp.outside""]').css('zIndex', 24);
			jQuery('div[id$=""_exp.inside_out""]').css('zIndex', 24);
			//console.log('banner set');
			clearInterval(banner_check2);
		}
	}, 1000);
	</script>

</head>

<body lang=""SR""


onunload=""unloadCallback(); ""  data-ip=""95.180.20.182"" data-user_id=""30101"">


<script type=""text/javascript"">
<!--//--><![CDATA[//><!--
var pp_gemius_identifier = new String('bDGQ1JAOv0L5Fj6cw7vvEnZG.jgNLW98Glsc_24r4cD.I7');
//--><!]]>
</script>
<script type=""text/javascript"" src=""http://www.polovniautomobili.com/javascript/xgemius.js""></script>


<div id=""fb-root""></div>
<script type=""text/javascript"">
// Load Facebook JS SDK asynchronously
window.fbAsyncInit = function() {
	FB.init({
		appId  : '127593208027',
		status : true, // check login status
		cookie : true, // enable cookies to allow the server to access the session
		xfbml  : true  // parse XFBML
		});

    /*FB.getLoginStatus(function(response) {
        if (response.status == 'unknown' && jQuery('#information iframe').length)
           {
           jQuery('#information iframe').css('height',208).parent().css('height',208);
           }
    }, true);*/
};
(function() {
	var e = document.createElement('script');
	e.src = document.location.protocol + '//connect.facebook.net/sr_RS/all.js';
	e.async = true;
	document.getElementById('fb-root').appendChild(e);
}());
</script>


<!-- JavaScript tag: PA pozadinski KOD unutrasnje, 7000606 -->
<script type=""text/javascript"">
// <![CDATA[
    var ord=Math.round(Math.random()*100000000);
    document.write('<sc'+'ript type=""text/javascript"" src=""http://ad.adverticum.net/js.prm?zona=7000606&ord='+ord+'""><\/scr'+'ipt>');
// ]]>
</script>
<noscript><a href=""http://ad.adverticum.net/click.prm?zona=7000606&nah=!ie"" target=""_blank"" title=""Click on the advertisement!""><img border=""0"" src=""http://ad.adverticum.net/img.prm?zona=7000606&nah=!ie"" alt=""Advertisement"" /></a></noscript>

<div class=""float-banner-left"">
	<!-- Goa3 tag: PA pozadinski unutrasnje LEVO, 7000603 -->
	<div id=""zone7000603"" class=""goAdverticum""></div>
</div>
<div class=""float-banner-right"">
	<!-- Goa3 tag: PA pozadinski unutrasnje DESNO, 7000604 -->
	<div id=""zone7000604"" class=""goAdverticum""></div>
</div>




<a name=""MountEverest""></a>

<div id=""waitingAjaxRequest"" style=""display: none; text-align: center;"">
	<br /><img src=""/images/bigrotation2x.gif"" alt=""Učitavanje"" /><br />- Učitavanje -
</div>
<div id=""overlaybox"" style=""display:none;"">
	<div id=""msgOverlay""></div>
	<div id=""msgClose"" class=""MB_done""><a href=""#"" id=""overlayboxClose"" onclick=""Modalbox.hide(); return false;"">Zatvori</a>
	</div>
</div>
<div id=""overlayConfirm"" style=""display: none;"">
	<div id=""msgConfirm""></div>
	<div id=""msgYesNo"" class=""MB_done""><input type=""button"" name=""overlayConfirmYes"" value=""Da"" class=""button"" />
		<input type=""button"" value=""Ne"" onclick=""Modalbox.hide(); return false;"" class=""button"" /></div>
</div>
<div id=""adminStatusReport"" style=""display: none;""></div>

<div id=""loginboxoverlay"" style=""display:none;"">
	<h2>Prijava</h2>

	<form class=""fullpage-form"" method=""post"" action="""">
		<fieldset class=""overlay"">
			<input type=""hidden"" id=""logintype"" value=""login"" />
			<div id=""errorlogin"" class=""error""></div>
			<div class=""set"">
				<label for=""username"">E-mail:</label>
				<input type=""text"" class=""text"" id=""username"" name=""email"" value="""" />
			</div>
			<div class=""set"">
				<label for=""password"">Šifra:</label>
				<input type=""password"" class=""text"" id=""password"" value="""" onkeyup=""if(event.keyCode == 13) tryToLogin('overlay-enter');"" />
			</div>
			<div class=""set"">
				<label for=""rememberme""><input type=""checkbox"" id=""rememberme"" class=""input_checkbox"" value=""yes"" />
					Zapamti me</label>
			</div>
            <table>
                <tr>
                    <td>
                        <div class=""set buttons"">
                            <input type=""button"" class=""button submit-button"" name=""submit"" value=""Prijava"" onclick=""$('errorlogin').style.display='none';tryToLogin('overlay-click'); var element = $('errorlogin'); new Effect.Appear(element); return false;"" />
                        </div>
                    </td>

                    <td>
                        <div>
                            <a href=""https://www.facebook.com/dialog/oauth?client_id=127593208027&redirect_uri=http%3A%2F%2Fwww.polovniautomobili.com%2Fmoj-profil.php%3Flogin%3Dfacebook%26fbloggedlink%3Dtrue&state=7d0749d462f92ef9ac5e89394f5d92b5&scope=email%2Cuser_location"">
                                <img src=""/images/fblogin-button.png"" style=""height:24px; width:101px; margin:-1px 0 0 -3px;"" />
                            </a>
                        </div>
                    </td>
                </tr>
            </table>
			<div class=""set"">
				Novi korisnik? <a href=""/registracija.php"">Registruj se!</a> |
				<a href=""#"" onclick=""Modalbox.hide(); Modalbox.show($('lostPassOverlay'));return false;"">Zaboravljena šifra?</a><br />
			</div>
			<div class=""set"">
				<a href=""#"" class=""close-overlay"" onclick=""Modalbox.hide(); return false;"">Odustani</a>
			</div>
		</fieldset>
	</form>
</div>

<div id=""main"" class=""extend"">

<div id=""service_links_row"" style=""width:1002px; height:20px; padding-top:4px;"">
	<div id=""service_links"" style=""float:left;font-size:11px;"">
		<ul>
			<li><a href=""/oglasi-na-mail.php"" title=""Primajte na vaš e-mail novopostavljene oglase vozila, vesti iz oblasti automobilizma i novosti na sajtu"" class="""">Oglasi na e-mail</a></li>
			<li><a href=""/knjiga-utisaka.php"" title=""Ostavite komentar na rad sajta i prenesite nam vaš opšti utisak"" class="""">Knjiga utisaka</a></li>
			<li><a href=""/wiki/cesto-postavljana-pitanja.php"" title=""Česta pitanja posetilaca"" class="""">Česta pitanja</a>
			</li>
			<li><a href=""http://www.infostud.com/za-medije/polovni-automobili/"" title=""Za medije"" class="""" target=""_blank"">Za medije</a>
			</li>
			<li><a href=""http://www.infostud.com/ponuda-za-predstavljanje-putem-banera-na-sajtu-polovniautomobili"" title=""Postavite baner na PolovniAutomobili.com"" class="""" target=""_blank"">Baneri</a></li>
			<li><a href=""/wiki/o-nama.php"" title=""Nešto više o nama..."" class="""">O nama</a></li>
			<li><a href=""/wiki/rss-i-mobilne-aplikacije.php"" title=""RSS sadržaj i aplikacija za Nokia mobilne telefone"" class="""">RSS i mob aplikacije</a>
			</li>
			<li>
				<a href=""/wiki/korisni-linkovi.php"" title=""Korisni linkovi - adrese sajtova koje preporučujemo"" class="""">Korisni linkovi</a>
			</li>
		</ul>
	</div>
	<div id=""loggedinbox"" style=""display: block;"">
		Dobrodošli  
		<a href=""/moj_profil.php"" rel=""nofollow"" style=""font-weight:bold"" title=""Kliknite ovde za pregled profila"">nemanja.simovic@gmail.com</a>
		<span class=""delimiter"">|</span>
		<a href=""#"" onclick=""tryToLogout(); return false;"" rel=""nofollow"" title=""Odjavite se iz sistema"">Odjavite se</a>
	</div>
	
</div>
<form action="""" method=""post"">
	<div id=""topline"" style=""background:#EC9C1E; height:92px; padding:5px 0;"">
		<div id=""logo"">
			<a href=""/"" title=""PolovniAutomobili.com - auto oglasi - prodaja novih i polovnih vozila""><img alt=""PolovniAutomobili.com - auto oglasi - prodaja novih i polovnih vozila"" src=""/images/polovniautomobili.com-logo-black.png"" /></a>
		</div>
		
		<div id=""main_banner"">
			<script type=""text/javascript"">
			// <![CDATA[
			    var ord=Math.round(Math.random()*100000000);
			    document.write('<sc'+'ript type=""text/javascript"" src=""http://ad.adverticum.net/js.prm?zona=7000555&ord='+ord+'""><\/scr'+'ipt>');
			// ]]>
			</script>
			<noscript><a href=""http://ad.adverticum.net/click.prm?zona=7000555&nah=!ie"" target=""_blank"" title=""Click on the advertisement!""><img border=""0"" src=""http://ad.adverticum.net/img.prm?zona=7000555&nah=!ie"" alt=""Advertisement"" /></a></noscript>
		</div>
		
		<div id=""logginbox"" style=""width:111px; float:right;"">

			<div id=""loginbox2"" style=""display: none;"">
				<div style=""width:100px"">
					<input type=""button"" class=""button"" value=""Prijava"" onclick=""tryToLoginFromHeader();"" />
					<label for=""remember"" class=""remember""><input type=""checkbox"" id=""rememberme_header"" class=""input_checkbox"" value=""yes"" />
						Zapamti me</label>
				</div>
				<div class=""links"">
					-<a href=""#"" onclick=""Modalbox.show($('lostPassOverlay')); return false;"" rel=""nofollow"">Zaboravljena šifra?</a><br>
					-<a href=""/registracija.php"" rel=""nofollow"" class=""register"">Registruj se!</a><br />
					<a href=""https://www.facebook.com/dialog/oauth?client_id=127593208027&redirect_uri=http%3A%2F%2Fwww.polovniautomobili.com%2Fmoj-profil.php%3Flogin%3Dfacebook%26fbloggedlink%3Dtrue&state=7d0749d462f92ef9ac5e89394f5d92b5&scope=email%2Cuser_location"">
					<img src=""/images/fblogin-button.png"" style=""height:24px; width:101px; margin:-1px 0 0 -3px;"" />
					</a>
				</div>
			</div>
			<div id=""logged_banner"" style=""display: block"">
                
			</div>
		</div>
		<div class=""clearboth""></div>
	</div>
</form>
<div class=""clearboth""></div>
<div id=""main-horiz-nav"">
	<div>
		<ul class=""megamenu"">
			<li><a href=""/"" title=""Naslovna stranica"" class="" selected"" style=""padding-top:4px; padding-bottom:2px;""><img src=""/images/home.png"" border=""0""></a></li>
			<li style=""background-image:url(/images/nova-vozila-linija.png);""><a class=""main "" href=""/putnicka-vozila-26.php?showold_pt=false&shownew_pt=true&showoldnew=new"" title=""Lista aktuelnih oglasa novih vozila"">
				Nova vozila
				<img src=""/images/mega-menu-arrow.png"" align=""absmiddle"" border=""0""></a>

				<div style=""width: 150px; left:43px"">
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-automobili"" title="""" class=""link"">Nova putnička vozila</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-motori"" title="""" class=""link"">Novi motori</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-kombi-i-laka-dostavna-vozila"" title="""" class=""link"">Novi kombiji i laka dostavna vozila</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/novi-kamioni-do-7-5t"" title="""" class=""link"">Nivo kamioni do 7t</a>
					</p>
					
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/nove-prikolice-poluprikolice"" title="""" class=""link"">Nove prikolice i poluprikolice</a>
					</p>
					
					<p class=""line-h"" style="""">
						<a href=""/nova-poljoprivredna-vozila"" title="""" class=""link"">Nova poljoprivredna vozila</a>
					</p>
					
				</div>
			</li>
			<li><a class=""main "" href=""/auto-prodavci.php"" title=""Spisak prodavaca koji svoju ponudu predstavljaju na sajtu"">
				Prodavci
				<img src=""/images/mega-menu-arrow.png"" align=""absmiddle"" border=""0""></a>

				<div style=""width: 150px; left:143px"">
					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/auto-dileri.php"" title="""" class=""link"">Dileri</a>
					</p>

					<p class=""line-h"" style=""border-bottom:#ccc 1px solid; margin-bottom:5px;"">
						<a href=""/auto-saloni.php"" title="""" class=""link"">Saloni</a>
					</p>

					<p class=""line-h"">
						<a href=""/auto-lizing.php"" title="""" class=""link"">Lizing kuće</a>
					</p>
				</div>
			</li>
			<li><a href=""/auto-usluge.php"" title=""Baza auto usluga"" class="""">Auto usluge</a></li>
			<li><a class=""main "" href=""/auto-vesti/"" title=""Razne vesti iz auto-moto industrije"">Auto vesti
				<img src=""/images/mega-menu-arrow.png"" align=""absmiddle"" border=""0""></a>

				<div style=""width: 860px; left:207px"">
					<div style=""width:160px; float:left; margin-right:10px"">
						<a href=""/auto-vesti/saveti"" title=""Saveti"" class=""title shadow"">Saveti</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/kako-izabrati-polovni-automobil.php"" class=""link"">??k? izabrati polovni automobil - šta gla...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/video-saveti-za-kupovinu-polovnog-automobila-ii-deo.php"" class=""link"">Video saveti za kupovinu polovnog automobila...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/video-saveti-za-kupovinu-polovnog-automobila-i-deo.php"" class=""link"">Video saveti za kupovinu polovnog automobila...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/saveti/kako-odrediti-cenu-automobila-koji-prodajete.php"" class=""link"">Kako odrediti cenu automobila koji prodajete...</a></p>
							
						</div>
						<a href=""/auto-vesti/saveti"" title=""Saveti"" class=""link other"">Svi saveti &raquo;</a>
					</div>
					<div style=""width:160px; float:left;"">
						<a href=""/auto-vesti/aktuelno/"" title=""Aktuelno"" class=""title shadow"">Aktuelno</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/tv-emisija-abs-show-i-sajt-www-polovniautomobili-com.php"" class=""link"">Video izveštaji sa sajma automobila u Be...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/kia-rio-1-2-cvvt-vs-kia-rio-1-4-crdi.php"" class=""link"">Rio - crno na belo</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/saradnja-sajtova-polovniautomobili-com-i-subotica-com.php"" class=""link"">Saradnja sajtova polovniautomobili.com i...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/aktuelno/abs-show-251.php"" class=""link"">ABS Show 251</a></p>
							
						</div>
						<a href=""/auto-vesti/aktuelno/"" title=""Aktuelno"" class=""link other"">Sve aktuelnosti &raquo;</a>
					</div>
					<div style=""width:160px; float:left; margin-left:10px"">
						<a href=""/auto-vesti/zanimljivosti/"" title=""Zanimljivosti"" class=""title shadow"">Zanimljivosti</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/vw-grupa-krupnim-koracima-napred.php"" class=""link"">VW Grupa krupnim koracima napred</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/2016-audi-a-5-novi-detalji.php"" class=""link"">2016 Audi A5 novi detalji</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/zeleznice-oglasile-prodaju-103-automobila.php"" class=""link"">????????? ???????? ??????? 103 ?????????...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/zanimljivosti/blindirani-mercedes-m-klase.php"" class=""link"">Blindirani Mercedes M klase</a></p>
							
						</div>
						<a href=""/auto-vesti/zanimljivosti/"" title=""Zanimljivosti"" class=""link other"">Sve zanimljivosti &raquo;</a>
					</div>
					<div style=""width:160px; float:left; margin-left:10px"">
						<a href=""/auto-vesti/tuning/"" title=""Tuning"" class=""title shadow"">Tuning</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/folksvagen-xl1-hibrid-super-stedisa.php"" class=""link"">Folksvagen XL1"" - hibrid super štediša</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/novobeogradski-tuning-styling-show.php"" class=""link"">Novobeogradski Tuning Styling Show</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/kicherer-tjunirani-mercedes-benzsls-ammg.php"" class=""link"">Kicherer: tjunirani Mercedes-Benz SLS AM...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/tuning/kreativne-ideje-mercedes-benz-a.php"" class=""link"">Kreativne ideje “Mercedes Benz”-a</a></p>
							
						</div>
						<a href=""/auto-vesti/tuning/"" title=""Tuning"" class=""link other"">Sve o tuningu &raquo;</a>
					</div>
					<div style=""width:160px; float:left; margin-left:10px"">
						<a href=""/auto-vesti/noviteti/"" title=""Noviteti"" class=""title shadow"">Noviteti</a>

						<div style="" height:140px;"">
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/ogromna-potraznja-za-ford-focus-st-prodavaniji-od-golf.php"" class=""link"">Ogromna potražnja za Ford Focus ST, proddavaniji...</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/predstavljen-renault-clio-gt.php"" class=""link"">Predstavljen Renault Clio GT</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/audi-a3-sportback-g-tron.php"" class=""link"">Audi A3 Sportback G-Tron</a></p>
							
							<p class=""line-h""><a href=""/auto-vesti/noviteti/predstavljen-vw-golf-gtd.php"" class=""link"">Predstavljen VW Golf GTD</a></p>
							
						</div>
						<a href=""/auto-vesti/noviteti/"" title=""Noviteti"" class=""link other"">Svi noviteti &raquo;</a>
					</div>
					<div class=""cb""></div>
				</div>
			</li>
			<li>
				<a class=""main"" href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=link_meni_pa"" title=""Internet prodaja guma"" target=""_blank"">Kupite gume
					<img src=""/images/ipg-logo-mm-mini.png"" align=""absmiddle"" border=""0""></a>
				<div style=""width: 665px; left:233px; overflow:hidden;"">
						<div style=""text-align:center; background-color:#33383C; border-radius:5px; margin-bottom:5px;""><a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni"" target=""_blank""><img src=""/images/ipg-logo-mm.png""></a></div>
					<div style=""width:150px; float:left;"">
						<a href=""http://www.internet-prodaja-guma.com/gume-na-akciji/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_gume_na_akciji"" class=""title shadow"" target=""_blank"">Zimske gume na akciji</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[6]=zimska&extra_fields[9]=MICHELIN&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_michelin"" class=""link"" target=""_blank"">Michelin </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[6]=zimska&extra_fields[9]=BF+GOODRICH&extra_fields[14]=Putni%C4%8Dko+vozilo&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_bf_goodrich "" class=""link"" target=""_blank"">BF Goodrich </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[6]=zimska&extra_fields[9]=SAVA&extra_fields[14]=Putni%C4%8Dko+vozilo&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_sava"" class=""link"" target=""_blank"">Sava </a></p>
						</div>
					</div>
					<div style=""width:150px; float:left; margin-left:10px"">
						<a href=""http://www.internet-prodaja-guma.com/gume-na-akciji/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_gume_na_akciji"" class=""title shadow"" target=""_blank"">Letnje gume na akciji</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&by_title=Y&by_shortdescr=Y&by_fulldescr=Y&by_sku=Y&including=all&substring=cordiant+comfort+ps400&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_cordiant"" class=""link"" target=""_blank"">Cordiant</a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/zeetex?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_zeetex"" class=""link"" target=""_blank"">Zeetex</a></p>
						</p>
						</div>
					</div>

	<div style=""width:165px; float:left; margin-left:10px"">
						<a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_najprodavanije_dimenzije"" class=""title shadow"" target=""_blank"">Najprodavanije dimenzije</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&by_title=Y&by_shortdescr=Y&by_fulldescr=Y&by_sku=Y&including=all&substring=205%2F55+R16&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_205_55_r16"" class=""link"" target=""_blank"">205/55 R16</a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&by_title=Y&by_shortdescr=Y&by_fulldescr=Y&by_sku=Y&including=all&substring=195%2F65+R15&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_195_65_r15"" class=""link"" target=""_blank"">195/65 R15 </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/search.php?mode=search&extra_fields[1]=225&extra_fields[2]=45&extra_fields[3]=17&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_225_45_r17"" class=""link"" target=""_blank"">225/45 R17 </a></p>
						</div>
					</div>

					<div style=""width:150px; float:left; margin-left:10px"">
						<a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_pogodnosti"" class=""title shadow"" target=""_blank"">Pogodnosti</a>

						<div>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/pages.php?pageid=17&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_gume_za_firme"" class=""link"" target=""_blank"">Gume za firme</a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/pages.php?pageid=15&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_popust_pri_montazi"" class=""link"" target=""_blank"">Popusti pri montaži </a></p>
						<p class=""line-h""><a href=""http://www.internet-prodaja-guma.com/help.php?section=contactus&mode=update&utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_link_meni_upit_za_gume"" class=""link"" target=""_blank"">Upit za gume </a></p>
						</div>
					</div>

				</div>
			</li>
			<li><a class=""main"" href=""http://www.mojagaraza.rs/?utm_source=polovniautomobili.com&utm_medium=link_meni&utm_campaign=pa_mg"" target=""_blank"">Diskutujte na <img src=""/images/moja-garaza-icon.png"" width=""14"" align=""absmiddle"" border=""0""></a>
				<div style=""width: 640px; left:233px; overflow:hidden;"">

						<div style=""text-align:center; background-color:#33383C; border-radius:5px; margin-bottom:5px;""><a href=""http://www.mojagaraza.rs"" target=""_blank""><img src=""/images/moja-garaza-logo.png"" ></a></div>
					<div style=""width:200px; float:left;"">
						<a href=""http://www.mojagaraza.rs/#!filter-reviews"" class=""title shadow"" target=""_blank"">Ako planirate kupovinu</a>

						<div>
						<img src=""/images/icons/potrazi-pa.png"" style=""position:absolute; top:36px; left:180px;"">
						<p class=""line-h""><a href=""http://www.mojagaraza.rs/#!filter-reviews"" class=""link"" target=""_blank"">Pogledajte ocene i utiske drugih vozača poput vas.</a></p>
						</div>
					</div>
					<div style=""width:200px; float:left; margin-left:10px"">
						<a href=""http://www.mojagaraza.rs/#!filter-questions"" class=""title shadow"" target=""_blank"">Pitanja i saveti</a>

						<div>
						<img src=""/images/icons/pitanja-pa.png"" style=""position:absolute; top:36px; left:390px;"">
						<p class=""line-h""><a href=""http://www.mojagaraza.rs/#!filter-questions"" class=""link"" target=""_blank"">Ako imate kvar ili neki drugi problem postavite pitanje, majstori odgovaraju.</a></p>
						</p>
						</div>
					</div>
					<div style=""width:200px; float:left; margin-left:10px"">
						<a href=""http://www.mojagaraza.rs/#!filter-galleries"" class=""title shadow"" target=""_blank"">Fotogalerije</a>

						<div>
						<img src=""/images/icons/fotogalerije-pa.png"" style=""position:absolute; top:36px; left:600px;"">
						<p class=""line-h""><a href=""http://www.mojagaraza.rs/#!filter-galleries"" class=""link"" target=""_blank"">Kupili ste super automobil, pokažite ga. Proverite šta misle drugi o vašem vozilu.</a></p>
						</div>
					</div>

				</div>


			</li>

			<li><a href=""/oglas/novi.php"" title=""Oglasite prodaju vozila, mašina, plovila, bicikala, delova i opreme"" rel=""nofollow"" class="""">Postavite oglas</a>
			</li>
			<li>
				<a href=""/moj-profil.php"" title=""Pogledajte vaš profil - vozila koja trenutno oglašavate, listu isteklih oglasa..."" rel=""nofollow"" class="""">Moj profil</a>
			</li>
			<li style=""border-right:none"">
				<a href=""/ponuda-za-oglasavanje.php"" title=""Izaberite odgovarajući paket i predstavite vašu ponudu"" class="""">Ponuda za oglašavanje</a>
			</li>


		</ul>
	</div>
</div>


<!--[if lte IE 6]>
<div id=""ie6block"" style=""background-color:#FFF9D4; font-size: 12px; font-weight: bold; color:#666666; border:#FFCC00 1px solid; margin:20px;"">
	<table cellpadding=""5"" cellspacing=""5"" style=""background-color:#FFF9D4;"">
		<tr align=""left"" style=""background-color:#FFF9D4;"">
			<td style=""width:100%;background-color:#FFF9D4;"">Ovaj sajt ne podrĹľava Internet Explorer 6 ili starije
				verzije!<br />
				<br />
				Molim Vas da preuzmete neki od ponuÄ‘enih modernijih Internet pregledaÄŤa.<br>
				<br />
				ViĹˇe o ovome na sledeÄ‡oj stranici:
				<br />
				<a href=""/wiki/ukidamo_podrsku_za_internet_explorer_6.php"" target=""_blank"" onClick=""var date = new Date();date.setTime(date.getTime()+(24*60*60*1000));document.cookie = 'ie6block=1; expires=' + date.toGMTString() + '; path=/';"" class=""blue-link"">
					<strong>Uskoro ukidamo podrĹˇku za Internet Explorer 6</strong> </a></td>
			<td style=""width:60%;background-color:#FFF9D4;"">
				<table border='0' align='center' cellpadding=""3"" cellspacing=""0"" style=""border:#999999 1px solid; background-color: #fff;"">
					<tr align='center'>
						<td colspan=""4"">
							<div align=""left"">PodrĹľavani Internet pretraĹľivaÄŤi</div>
						</td>
					<tr align='center'>
						<td><a href='http://www.mozilla.com/sr/'> <img border='0' src='/images/ie6block/firefox.gif' />
						</a></td>
						<td><a href='http://www.google.com/chrome'> <img border='0' src='/images/ie6block/chrome.gif' />
						</a></td>
						<td><a href='http://www.opera.com/'> <img border='0' src='/images/ie6block/opera.gif' /> </a>
						</td>
						<td><a href='http://www.microsoft.com/windows/internet-explorer/default.aspx'>
							<img border='0' src='/images/ie6block/ie.gif' /> </a></td>
					<tr align='center'>
						<td>Firefox</td>
						<td>Google Chrome
						</td>
						<td>Opera</td>
						<td>Internet Explorer 8</td>
					</tr>
				</table>
			</td>
			<td valign=""top"" style=""background-color:#FFF9D4;"">
				<a href="""" onClick=""document.getElementById('ie6block').style.display = 'none'; var date = new Date();date.setTime(date.getTime()+(24*60*60*1000));document.cookie = 'ie6block=1; expires=' + date.toGMTString() + '; path=/'; return false;"">
					<div title=""IgnoriĹˇi upozorenje"" style=""width:20px; line-height:20px; text-align:center; color:#000000; font-weight:bold; background-color:#FFFFFF; border:#999999 1px solid; cursor:pointer"">
						X</div>
				</a></td>
		</tr>
	</table>
</div>
<![endif]-->


<div class=""clearboth""></div>
<div id=""content"" class="""">
	<div id=""page"">
		<a name=""searchbox""></a>

		<div id=""navigation"" class=""extend"">
			
			<div id=""mainnav"" class=""main"">
				<ul>
					<li style=""width:108px"" class=""selected"">
						<a href=""/"" class=""carselected"">Putnička vozila</a></li>
					<li style=""width:64px"" class="""">
						<a href=""/motori.php"" class=""moto"">Motori</a></li>
					<li style=""width:125px"" class="""">
						<a href=""/kombi-i-laka-dostavna-vozila.php"" id=""transport_menuitem"" class=""transport"">Transportna vozila</a>
					</li>
					<li style=""width:136px"" class="""">
						<a href=""/poljoprivredna-vozila.php"" class=""agro"">Poljoprivredna vozila</a></li>
					<li style=""width:105px"" class="""">
						<a href=""/radne-masine.php"" class=""working"">Radne mašine</a></li>
					<li style=""width:80px"" class="""">
						<a href=""/plovila.php"" class=""nauti"">Plovila</a></li>
					<li style=""width:111px"" class="""">
						<a href=""/delovi-oprema.php"" class=""parts"">Delovi i oprema</a></li>
					<li style=""width:59px"" class="""">
						<a href=""/bicikli.php"" class=""bike"">Bicikli</a></li>
				</ul>
			</div>
			<div class=""cb""></div>
			<div id=""transport_menu"" style=""display:none;"">
				<ul style=""margin: 0 0 0 10px"">
					<li class=""""><a href=""/kombi-i-laka-dostavna-vozila.php"" class=""kombi"">Kombi i laka dostavna vozila</a>
					</li>
					<li class="""">
						<a href=""/kamioni-do-7-5t.php"" class=""mali-kamioni"">Kamioni do 7.5t</a></li>
					<li class=""""><a href=""/kamioni-preko-7-5t.php"" class=""veliki-kamioni"">Kamioni preko 7.5t</a>
					</li>
					<li class=""""><a href=""/prikolice-poluprikolice.php"" class=""prikolice"">Prikolice i poluprikolice</a></li>
					<li class=""""><a href=""/autobusi.php"" class=""autobusi"">Autobusi</a>
					</li>
					<li class=""""><a href=""/kamperi.php"" class=""kamperi"">Kamperi</a>
					</li>
				</ul>
			</div>
			
			
		</div>
		
		<div id=""main_content"" class=""main_content ""
		>
		

<div id=""msgboxoverlay"" style=""display: none;"">
	<h2>Pošaljite poruku prodavcu</h2>
	<form class=""fullpage-form"" method=""post"" action="""">
		<fieldset class=""overlay"">
			<input type=""hidden"" id=""msg_classified"" value="""" />
			<input type=""hidden"" id=""msg_touser""  value="""" />
			<div id=""msg_error"" class=""error""></div>
			
			<div class=""set"">
				<label for=""msg_name"">Ime i prezime: <em class=""asterisk"">*</em></label>
				<input type=""text"" class=""text"" id=""msg_name"" value=""Marko Simovi"" />
			</div>
			<div class=""set"">
				<label for=""msg_email"">E-mail: <em class=""asterisk"">*</em></label>
				<input type=""text"" class=""text"" id=""msg_email"" value=""nemanja.simovic@gmail.com"" />
			</div>
			<div class=""set"">
				<label for=""msg_phone"">Telefonski broj:</label>
				<input type=""text"" class=""text"" id=""msg_phone"" value=""064/8266468"" />
			</div>
			<div class=""set"">
				<label for=""msg_message"">Poruka: <em class=""asterisk"">*</em></label>
				<textarea class=""text"" id=""msg_message"" name=""msg_message"" cols=""50"" rows=""4""></textarea>
			</div>
			
			<div class=""set buttons"">
				<input type=""button"" name=""submit"" class=""button"" value=""POŠALJI"" class=""submit-button"" onclick=""tryToSendMessage(); return false;"" />
			</div>
			<div class=""set"">
				<a href=""#"" onclick=""Modalbox.hide(); return false;"">Odustani</a>
			</div>
		</fieldset>
	</form>
</div>


<style type=""text/css"">
	.page_content {
		padding-right: 0;
		padding-left: 0;
		}
	#searchbox_container .page_content {
		padding: 5px 10px;
		}

	.box {
		background-color: #fff;
		border-radius: 5px 5px 5px 5px;
		box-shadow: 0 0 4px #999;
		margin: 0 0 6px;
		padding: 6px 8px;
		border:none;
		width:280px;
		border:1px solid #CCC;
		}
	
	.tab_holder {
		width: 100%;
		position: absolute;
		top:-36px;
		}
	.tab_holder.tabs_5 .tab {
		width: 128.8px;
		}
	.tab_holder.tabs_4 .tab {
		width: 161.5px;
		}
	.tab_holder.tabs_3 .tab {
		width: 216px;
		}
	.tab_holder.tabs_2 .tab {
		width: 325px;
		}
	.tab_holder.tabs_1 .tab {
		width: 325px;
		}
	a.tab {
		border-top: 2px solid #d7d7d7;
		border-left: 1px solid #d7d7d7;
		border-right: 1px solid #d7d7d7;
		border-radius:5px 5px 0 0;
		float:left;
		text-align:center;
		display:block;
		text-decoration:none;
		height:35px;
		line-height:35px;
		font-size:14px;
		}
	a.tab {
		background-image:url(/images/tab_color.jpg);
		background-repeat:repeat-x;
		color:#404040;
		border-bottom: 1px solid #d7d7d7;
		}
	a.tab:hover {
		background-color:#fff;
		background-position:5px 5px;
		color:#EC9C1E !important;
		}
	a.tab.active {
		color:#000;
		background-image:none;
		background-color:#fff;
		font-weight:bold;
		border-bottom:none;
		}
	#tab_bg {
		background-color: #fff;
		clear:both;
		padding:10px;
		border: solid 1px #d7d7d7;
			margin-top:40px;
		}
	.dealer_info {
		width: 270px;
		height:280px;
		background-image:url(/images/background_projekat.png);
		background-repeat:repeat-x;
		margin-bottom:10px;
		float:left;
		color:#404040;
		}
	.dealer_contact {
		width: 317px;
		min-height:280px;
		float: right;
		background-image:url(/images/background_projekat.png);
		background-repeat:repeat-x;
		margin-bottom:10px;
		}
	.dealer_description {
		clear: both;
		width: 602px;
		height:auto;
		margin-bottom:10px;
		color:#404040; 
		line-height:22px; 
		padding:15px;
		text-align:justify; 
		}
	.dealer_description.narrow {
		clear: none;
		width:305px;
		height:262px;
		margin-bottom:10px;
		color:#626160;
		text-align:left;
		float:right;
		overflow: auto;
		}
	.dealer_map {
		clear:both;
		width:616px;
		height:420px;
		}
	.dealer_map .canvas {
		width: 616px;
		height: 420px;
		}
	.dealer_map_description .label {
		display: inline-block;
		width: 60px;
		font-weight: bold;
		}
	#msg_error {
		padding: 10px;
		line-height: 17px;
		}
	.button_contact {
		border:none;float:right; 
		padding-right:2px; 
		margin-top:7px; 
		width: 80px; 
		height: 26px; 
		background-image:url(/images/dugme-posalji.png); 
		background-color:transparent;
		background-repeat:no-repeat;
		background-position:0 -2px;
		}
	.button_contact:hover {
		cursor: pointer;
		background-position:0 -29px;
		}
	.registration_box { 
	background-color: #FFFFFF;
	border: 1px solid #CCCCCC;
	border-radius: 5px 5px 5px 5px;
	box-shadow: 0 0 4px #999999;
	margin: 0 0 6px;
	padding: 6px 8px;
	width: 312px;
		}
	#searchbox_container .page_content {
		border: none;
		border-radius:none;
		margin-left:-10px;
		width:632px;
		background:none
		}
	#searchbox_container .corner {
		display:none;
		}
.box-title-orange label {
	color:#FFFFFF;
	}
.button_search {
	background: url(/images/dugme-trazi.png) no-repeat 0 -2px;
 	width: 80px; 
	height: 26px;
	float:right;
	}
.button_search:hover {
	background: url(/images/dugme-trazi.png) no-repeat 0 -29px;
 	width: 80px; 
	height: 26px;
	}
.pretraga-table label {
	display: inline-block;
	width: 70px;
	}
</style>

<div id=""frame"" class=""extend "">
	<div id=""salon_details"" class=""extend"">
		<div class=""additional"">
			
			<div class=""information-2"" id=""useful_links"" style=""border-radius:5px 5px 0px 0px; -moz-border-radius:5px 5px 0px 0px; -webkit-border-radius:5px 5px 0px 0px;"">
				<div class=""corner bl""></div>
				<div class=""corner br""></div>
				<div class=""box-title-maroon"" style=""border-radius:5px 5px 0px 0px; -moz-border-radius:5px 5px 0px 0px; -webkit-border-radius:5px 5px 0px 0px;"">
					<h1>Korisni linkovi</h1></div>
					<div class=""content"">
						
						<a href=""javascript:history.go(-1);"" onclick=""history.go(-1); return false;"" rel=""nofollow"" class=""back""><strong>Povratak</strong></a>
						
						
						
							<a href=""/lk-team"" title=""Ostali oglasi oglašivača"" class=""list""><div style='height:4px'></div>
Ostali oglasi oglašivača </a><a href=""#"" title=""Sačuvaj oglas u svom profilu"" class=""menuStar"" id=""classifiedPicker"" onclick=""togglePicked('on', 3116564); return false;"" rel=""nofollow""><div style='height:4px'></div>
Prati oglas </a><a href=""#"" title=""Prijavi nepravilnost u oglasu"" class=""report"" onclick=""reportClassified(3116564); return false;""  rel=""nofollow""><div style='height:10px'></div>
Prijavite oglas </a><a href=""http://www.polovniautomobili.com/oglasi-na-mail.php"" title=""Primajte oglase na e-mail"" class=""letter"" >Primajte oglase na e-mail </a><a href=""#"" title=""Pošalji oglas prijatelju"" class=""sendfriend"" onclick=""sendToFriend(3116564, '2005 Smart ForFour 1.3 | Polovni automobili'); return false;""  rel=""nofollow""><div style='height:4px'></div>
Pošaljite oglas prijatelju </a><a href=""javascript:void(0);"" title=""Štampaj oglas"" class=""print"" onclick=""window.print()""  rel=""nofollow""><div style='height:10px'></div>
Štampajte </a><a href=""/docs/kupoprodajni-ugovor.pdf"" class=""contract"">Preuzmite kupoprodajni ugovor </a>
						
					</div>
				<div class=""corner bl""></div>
				<div class=""corner br""></div>	
			</div>
			
		</div>
		
		<div id=""listing"" class=""page_content extend"" style=""margin-top:-5px; margin-left:0px; width:654px; ""  data-params='{""brand"":198,""model"":1891,""year"":2005,""body_style"":277,""classified_id"":3116564}'>
			<div class=""box-title-maroon"">
				<h1>
					<div style=""float:left;"">2005 Smart ForFour 1.3</div>
					
					
					<div style=""float:right; font-size:16px"">2.850 &euro;</div>
					
					
					
					
					<div style=""clear:both""></div>
				</h1>
			</div>
			
			
			<div id=""socialshare2"" style=""margin:10px;"">
				<div style='width:110px; float:left;'>
					<fb:like href=""http://www.polovniautomobili.com/oglas3116564/smart_forfour_13/"" send=""false"" width=""320"" show_faces=""false"" action=""recommend"" layout=""button_count""></fb:like>
					<script type=""text/javascript"">
					FB.Event.subscribe('edge.create', function(response) {
						_gaq.push(['_trackEvent', 'fb-like-oglas-gore', 'click', '2005 Smart ForFour 1.3 | Polovni automobili ']);
					});
					</script>
				</div>
				
				<div style='width:100px; float:right;'>
					<a href=""http://twitter.com/share"" class=""twitter-share-button"" data-count=""horizontal"">Tweet</a>
					<script type=""text/javascript"" src=""http://platform.twitter.com/widgets.js""></script>
				</div>
				
				<div style='width:65px; float:right;'>
					<g:plusone size=""medium""></g:plusone>
					<script type=""text/javascript"">
					 window.___gcfg = {lang: 'sr'};
					 (function() {
						var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
						po.src = 'https://apis.google.com/js/plusone.js';
						var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
					})();
					</script>
				</div>
	
				<div style='float:right; margin-top:-3px;'>
					<img src=""/images/fb-pitaj-prijatelja-dugme.png"" onclick=""fb_ask_friends();return false;"" alt=""Pitaj prijatelje"" style=""margin:0;cursor:pointer"" title=""Pitajte va?e prijatelje da li da kupite ovo vozilo"" />
				</div>
				<script>
				function fb_ask_friends() {
					// Track the click
					_gaq.push(['_trackEvent', 'Pitajte na FB', 'click', 'Smart ForFour 1.3, 2005 god, 2.850 &euro;']);
					// calling the API ...
					var obj = {
						method: 'send',
						link: 'http://www.polovniautomobili.com/oglas3116564/smart_forfour_13/',
						picture: 'http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-400x300.jpg',
						name: 'Da li mislite da je ovo vozilo dobra kupovina?',
						caption: '<strong>Smart ForFour 1.3, 2005 god, 2.850 &euro;</strong>',
						description: 'Prešao: 98250 km, Boja: Siva metalik<center></center>4/5 vrata, 5 sedišta<center></center>Benzin, 70kW, 95KS'
					};
					function callback(response) {
						if (response && response.post_id) {
						_gaq.push(['_trackEvent', 'Pitajte na FB - Submit', 'click', 'Smart ForFour 1.3, 2005 god, 2.850 &euro;']);
						}
					}
					FB.ui(obj, callback);
				}
				  </script>
				<div style=""clear:both;""></div>
			</div><!--#socialshare2-->
			
<div style=""position:relative"">
			
			<div class=""tab_holder tabs_5"">
				
				<a href=""?"" class=""tab active"">Oglas</a>
				
				<a href=""?prikaz=o-modelu"" class=""tab track"">O modelu</a>
				
				<a href=""?prikaz=utisci-vozaca"" class=""tab track"">Utisci vozača</a>
				
				<a href=""?prikaz=finansiranje"" class=""tab track"">Finansiranje</a>
				
				<a href=""?prikaz=kontakt"" class=""tab track"">Kontakt</a>
				
			</div>
			

			<div id=""tab_bg"">
			
				
				<div class=""info"" style=""width:296px; margin-right: 8px;"">
					<div class=""box"">
						

						<h2>Opšte informacije</h2>
						<table width=""100%"" id=""tbl-details"">
						
							
							<tr>
								<td class=""tdl"">Vozilo:</td>
								<td class=""tdr""> Polovno</td>
							</tr>
							
							<tr>
								<td class=""tdl"">Marka:</td>
								<td class=""tdr""> Smart</td>
							</tr>
							
							<tr>
								<td class=""tdl"">Model:</td>
								<td class=""tdr""> ForFour</td>
							</tr>
							

							
								

								
									
									<tr>
										<td class=""tdl"">Godina proizvodnje:</td>
										<td class=""tdr""> 2005</td>
									</tr>
									
									<tr>
										<td class=""tdl"">Karoserija:</td>
										<td class=""tdr""> Limuzina</td>
									</tr>
									
									<tr>
										<td class=""tdl"">Gorivo:</td>
										<td class=""tdr""> Benzin</td>
									</tr>
									
								
							

							
							<tr>
								<td class=""tdl"">Fiksna cena:</td>
								<td class=""tdr""> NE</td>
							</tr>
							

							
							<tr>
								<td class=""tdl"">Zamena:</td>
								<td class=""tdr""> NE</td>
							</tr>
							

							

							

							

							
							<tr>
								<td class=""tdl"">Datum postavljanja:</td>
								<td class=""tdr""> 18.03.2013.</td>
							</tr>
							
							<tr>
								<td class=""tdl"">Broj oglasa:</td>
								<td class=""tdr""> 3116564</td>
							</tr>
							
						
						</table>
					</div>


					

					
						

						

						
						<div class=""box"">
							<h2>Dodatne informacije</h2>
							<table width=""100%"" id=""tbl-details"">
								
								<tr>
									<td id=""tagdt_136"" class=""tdl"">Kubikaža (cm3):</td>
									<td id=""tagdd_136"" class=""tdr""> 1332</td>
								</tr>
								
								<tr>
									<td id=""tagdt_137"" class=""tdl"">Snaga (kW):</td>
									<td id=""tagdd_137"" class=""tdr""> 70.00</td>
								</tr>
								
								<tr>
									<td id=""tagdt_131"" class=""tdl"">Kilometraža:</td>
									<td id=""tagdd_131"" class=""tdr""> 98250</td>
								</tr>
								
								<tr>
									<td id=""tagdt_3252"" class=""tdl"">Emisiona klasa motora:</td>
									<td id=""tagdd_3252"" class=""tdr""> Euro 4</td>
								</tr>
								
								<tr>
									<td id=""tagdt_144"" class=""tdl"">Pogon:</td>
									<td id=""tagdd_144"" class=""tdr""> Prednji</td>
								</tr>
								
								<tr>
									<td id=""tagdt_138"" class=""tdl"">Menjač:</td>
									<td id=""tagdd_138"" class=""tdr""> Manuelni 5 brzina</td>
								</tr>
								
								<tr>
									<td id=""tagdt_140"" class=""tdl"">Broj vrata:</td>
									<td id=""tagdd_140"" class=""tdr""> 4/5 vrata</td>
								</tr>
								
								<tr>
									<td id=""tagdt_2701"" class=""tdl"">Broj sedišta:</td>
									<td id=""tagdd_2701"" class=""tdr""> 5 sedišta</td>
								</tr>
								
								<tr>
									<td id=""tagdt_2627"" class=""tdl"">Strana volana:</td>
									<td id=""tagdd_2627"" class=""tdr""> Levi volan</td>
								</tr>
								
								<tr>
									<td id=""tagdt_60"" class=""tdl"">Klima:</td>
									<td id=""tagdd_60"" class=""tdr""> Manuelna klima</td>
								</tr>
								
								<tr>
									<td id=""tagdt_58"" class=""tdl"">Boja:</td>
									<td id=""tagdd_58"" class=""tdr""> Siva</td>
								</tr>
								
								<tr>
									<td id=""tagdt_139"" class=""tdl"">Registrovan do:</td>
									<td id=""tagdd_139"" class=""tdr""> Nije registrovan</td>
								</tr>
								
								<tr>
									<td id=""tagdt_3046"" class=""tdl"">Poreklo vozila:</td>
									<td id=""tagdd_3046"" class=""tdr""> Na ime kupca</td>
								</tr>
								
								<tr>
									<td id=""tagdt_3796"" class=""tdl"">Oštećenje:</td>
									<td id=""tagdd_3796"" class=""tdr""> Nije oštećen</td>
								</tr>
								
							</table>
						</div>
						
					
						

						
						<div class=""box ndaddit"">
							<h2>Sigurnost</h2>
							<ul><li>- Airbag za vozača</li><li>- Airbag za suvozača</li><li>- Bočni airbag</li><li>- Child lock</li><li>- ABS</li><li>- ESP</li><li>- Kodiran ključ</li><li>- Blokada motora</li><li>- Centralno zaključavanje</li></ul>
							<div class=""cb""></div>
						</div>
						

						
					
						

						
						<div class=""box ndaddit"">
							<h2>Oprema</h2>
							<ul><li>- Metalik boja</li><li>- Branici u boji auta</li><li>- Servo volan</li><li>- Daljinsko zaključavanje</li><li>- Putni računar</li><li>- Tonirana stakla</li><li>- Električni prozori</li><li>- Električni retrovizori</li><li>- Grejači retrovizora</li><li>- Sedišta podesiva po visini</li><li>- Svetla za maglu</li><li>- Radio CD</li></ul>
							<div class=""cb""></div>
						</div>
						

						
					
						

						
						<div class=""box ndaddit"">
							<h2>Stanje vozila</h2>
							<ul><li>- Garažiran</li><li>- Rezervni ključ</li></ul>
							<div class=""cb""></div>
						</div>
						

						
					
					
							
						
					<div class=""box"">
						<h2>Opis</h2>
						<p>automobil u odlicnom stanju,motor trapovi,enterijer u odlicnom stanju .dva kljuca,ima malo ,ostecenje pukla je prednja maska sto se vidi na slikama.placeni carina i porez,kupcu ostaje samo registracija.</p>
					</div>
					
					<script type=""text/javascript"" defer=""defer"">
					var oldkwks = jQuery('#tagdd_137').html();
					if (oldkwks != null) {
						//oldkwks = oldkwks.replace(/\D/ig,'');
						oldkwks = parseFloat(oldkwks);
						if (!isNaN(oldkwks) && oldkwks > 0) {
							var newkwks = Math.round(oldkwks * 1.359621617);
							jQuery('#tagdd_137').html(' ' + Math.round(oldkwks) + ' / ' + newkwks + ' <br />');
							var oldkwksname = jQuery('#tagdt_137').html();
							var newkwksname = oldkwksname.replace(/kW/, 'kW / KS');
							jQuery('#tagdt_137').html(newkwksname);
						}
					}
					</script>

					
				</div><!--.info-->

				<div class=""additional"" style=""width:312px;"">
					
					<div id=""gallery"" class=""box extend ndaddit"" style=""width:312px;"">
						<h2>Slike (8)</h2>
						<div class=""ad-gallery small"">
							<div class=""ad-image-zoom""></div>
							<div class=""ad-image-wrapper""></div>
							<div class=""clear"">&nbsp;</div>
							<div class=""ad-nav"">
								<div class=""ad-thumbs"">
									<ul class=""ad-thumb-list"">
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-100x75.jpg"" class=""image0"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/7562aef2731b-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/7562aef2731b-100x75.jpg"" class=""image1"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/3534033ca1e7-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/3534033ca1e7-100x75.jpg"" class=""image2"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/300242bf1af0-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/300242bf1af0-100x75.jpg"" class=""image3"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/fc1097bd5f56-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/fc1097bd5f56-100x75.jpg"" class=""image4"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/9933132ab5af-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/9933132ab5af-100x75.jpg"" class=""image5"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/371d478bb143-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/371d478bb143-100x75.jpg"" class=""image6"">
											</a>
										</li>
										
										<li>
											<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/23e5d53c1f64-308x231.jpg"">
												<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/23e5d53c1f64-100x75.jpg"" class=""image7"">
											</a>
										</li>
										
									</ul><!--.ad-thumb-list-->
								</div><!--.ad-thumbs-->
							</div><!--.ad-nav-->
						</div><!--.ad-gallery small-->
					</div><!--#gallery-->

					<script>
					jQuery(function() {
						jQuery('.ad-gallery.small').adGallery({
							width:308,
							height: 231,
							thumb_opacity: 1,
							effect: 'none'
						});
						jQuery('.ad-thumbs').scrollLeft(500);
						jQuery('.ad-gallery.small .ad-image-zoom').click(function() {
							var thumb = jQuery('.ad-gallery.small .ad-active').parent();
							if (thumb.length > 0) {
								showImageGallery(false, thumb.index() );
							} else {
								showImageGallery(false, 0);
							}
						});
					});
					</script>
					

					<div id=""details-agency"" class=""box extend agency ndaddit"" style=""width:312px;"">
						
						<a style=""text-decoration:none;"" href=""/lk-team"">
							<img src=""/user-images/superadvertiser/244661/logo-small.jpg"" alt=""LKTEAM"" align=""right"" />
							<h3>LKTEAM</h3>
						</a>
						<a style=""text-decoration:none;"" href=""/lk-team"" rel=""nofollow"" title=""Pogledajte kompletnu ponudu"">Pogledajte kompletnu ponudu</a>
						<br />
						<br />
						

						<h2>Kontakt podaci</h2>
						<div style=""padding-top:6px; float:left;"">
							
							<span style=""float:right; font-weight:bold; padding-left:5px"">
								(<a href=""javascript:void(0);"" onclick=""OpenMap('oglas')"" id=""gmap"" onmouseover=""new Tip(this.id, 'Pogledajte lokaciju na mapi');"">Pogledajte mapu</a>)
							</span>
							Telep
							<br />

							<div style=""display:none"" id=""dealer_contact_map"">
								

								<span style=""width:370px; display:inline-block; font-weight:bold; margin-bottom:5px;"">LKTEAM</span>
								<span class=""name"">Adresa:</span>
								<span style=""width:310px"">Bajči Žilinskog 11<br /></span>

								
								<span class=""name"">Mesto:</span>
								<span>21000 Novi Sad<br /></span>
								

								

								
								<span class=""name"">Telefon:</span>
								<span>060/057-4429<br /></span>
								

								

								

								
								<span class=""name"">Mobilni:</span>
								<span>061/629-7401<br /></span>
								

								
							</div>
							

							
							21000 Novi Sad, 
							Srbija<br />
							Južno-bački okrug <br />
							Telefon 1: 060/057-4429<br />
							
							
							Mobilni: 063574424<br />
							
						</div>

						

						<div class=""clearboth""></div>

						
						<div class=""contact"" style=""text-align:right; margin-top:5px"">
							<a href=""javascript:void(0);"" onclick=""sendMessage(244661, 3116564, 'Smart ForFour 1.3', ''); return false;"" rel=""nofollow"">Kontaktirajte prodavca na e-mail</a>
						</div>
						
					</div><!--#details-agency-->
					
					
					<div class=""registration_box"">
						<div class=""tip"">
							<img src=""/images/icons/icon-help.png"" id=""itooltip999"" style=""margin:0px;"" title=""<div style='width:400px;''><div style='float:left; font-weight:bold; color:#B0122B'><img src='/images/icons/icon-help-red.png'> Info</div><div style='float:right'><a href='' onclick='closeToolTip(999); return false;'><img src='/images/icons/icon-close.png'></a></div><div style='clear:both; padding-top:5px;'><strong>U saradnji sa sajtom registracija-vozila.rs prikazujemo raspon ukupnih troškova registracije. Tačan iznos troška za Vašu opštinu možete pogledati na registracija-vozila.rs</strong></div></div>"" onload=""jQuery('#itooltip999').tooltip({ predelay: '400', effect: 'fade',direction: 'down',cancelDefault:true,offset:[-21,21],position:'bottom left',tipClass:'karoserijaTip'});"" /></div>
						<div>
							<img style=""margin:0px;"" src=""/images/rv-logo.jpg"" />
						</div>
						<table>
							<tr>
								<td>Cena registracije od</td>
								<td style=""padding-left:2px;""><strong>16.896</strong> din.</td>
								<td style=""padding-left:1px;"">do</span> <strong>18.819</strong> din.</td>
							</tr>
						</table>
						<a style=""text-decoration:none; float:right;"" title=""Pogledajte detaljne informacije na www.registracija-vozila.rs"" href=""http://www.registracija-vozila.rs/index.php?option=com_chronocontact&chronoformname=Info_za_registraciju&lang=sr&k=p&g=2005&z=1332&s=70"" target=""_blank"" onclick='_gaq.push([""_trackEvent"", ""Registracija Vozila"", ""Click"", ""Smart ForFour 1.3""]);'>Pogledajte tačan iznos</a>
						<div class=""cb""></div>
					</div>
					

					
					<div id=""ipg_search"" class=""ipg-box"" style=""width:312px;"">
						<div style=""width:150px; float:left;"">
							<label for=""ipg_width"">
								Širina
							</label>
							<select id=""ipg_width"" name=""x_quick[0]"">
								<option value="""">bilo koja</option>
								<option value=""6"">6</option><option value=""6.50"">6.50</option><option value=""7"">7</option><option value=""7.50"">7.50</option><option value=""10"">10</option><option value=""30"">30</option><option value=""31"">31</option><option value=""32"">32</option><option value=""33"">33</option><option value=""35"">35</option><option value=""37"">37</option><option value=""135"">135</option><option value=""145"">145</option><option value=""155"">155</option><option value=""165"">165</option><option value=""175"">175</option><option value=""185"">185</option><option value=""195"">195</option><option value=""205"">205</option><option value=""215"">215</option><option value=""225"">225</option><option value=""235"">235</option><option value=""245"">245</option><option value=""255"">255</option><option value=""265"">265</option><option value=""275"">275</option><option value=""285"">285</option><option value=""295"">295</option><option value=""305"">305</option><option value=""315"">315</option><option value=""325"">325</option><option value=""335"">335</option><option value=""345"">345</option><option value=""355"">355</option><option value=""365"">365</option><option value=""375"">375</option><option value=""385"">385</option><option value=""395"">395</option><option value=""405"">405</option><option value=""415"">415</option>
							</select>
							<br />

							<label for=""ipg_height"">
								Visina
							</label>
							<select id=""ipg_height"" name=""x_quick[1]"">
								<option value="""">bilo koja</option>
								<option value=""9.50"">9.50</option><option value=""10.50"">10.50</option><option value=""11.50"">11.50</option><option value=""12.50"">12.50</option><option value=""25"">25</option><option value=""30"">30</option><option value=""35"">35</option><option value=""40"">40</option><option value=""45"">45</option><option value=""50"">50</option><option value=""55"">55</option><option value=""60"">60</option><option value=""65"">65</option><option value=""70"">70</option><option value=""75"">75</option><option value=""80"">80</option><option value=""82"">82</option>
							</select>
							<br />

							<label for=""ipg_radius"">
								Prečnik (R)
							</label>
							<select id=""ipg_radius"" name=""x_quick[2]"">
								<option value="""">bilo koji</option>
								<option value=""10"">10</option><option value=""12"">12</option><option value=""13"">13</option><option value=""14"">14</option><option value=""15"">15</option><option value=""16"">16</option><option value=""17"">17</option><option value=""18"">18</option><option value=""19"">19</option><option value=""20"">20</option><option value=""21"">21</option><option value=""22"">22</option><option value=""23"">23</option>
							</select>
							<br />

							
							<label for=""ipg_season"">
								Sezona
							</label>
							<select id=""ipg_season"" name=""x_quick[4]"">
								<option value="""">bilo koja</option>
								<option value=""letnja"">letnja</option>
								<option value=""za sve sezone"">za sve sezone</option>
								<option value=""zimska"">zimska</option>
							</select>
							<br />
							

							<a href=""javascript:void(0);"" onclick=""IPGSearch('box_pa_automobili_dole',false,26)"" class=""ipg-search"">Pronađi gume</a>
						</div>

						<div style=""float:right;"">
							<div style=""text-align:center; margin-bottom:24px;"">
								<a href=""http://www.internet-prodaja-guma.com/?utm_source=polovniautomobili.com&utm_medium=logo&utm_campaign=box_pa_automobili_dole"" target=""_blank"">
									<img src=""/images/ipg-logo-manji.png"" border=""0"" alt=""Internet Prodaja Guma"" />
								</a>
							</div>
							<div id=""ipg_designs_wide"">
								<ul>

<li><a href=""http://www.internet-prodaja-guma.com/auto-gume/debica-205-60-r15-furio-91v-dot2010/dot507265/?xxxUTMxxx"" target=""_blank""><img src=""http://polovniautomobili.com/images/ipg/furio.gif"" />DEBICA 205/60 R15 FURIO 91V (DOT2010)<br/>3999din</a></li>

<li><a href=""http://www.internet-prodaja-guma.com/auto-gume/goodyear-185-55-r14-duragrip-80h-dot2010/dot520503/?xxxUTMxxx"" target=""_blank""><img src=""http://polovniautomobili.com/images/ipg/duragrip.gif"" />GOODYEAR 185/55 R14 DURAGRIP 80H (DOT2010)<br/>4999 din</a></li>

<li><a href=""http://www.internet-prodaja-guma.com/auto-gume/fulda-175-70-r14-carat-attiro-84h-tl-dot2010/dot508763/?xxxUTMxxx"" target=""_blank""><img src=""http://polovniautomobili.com/images/ipg/attiro.gif"" />FULDA 175/70 R14 CARAT ATTIRO 84H TL (DOT2010)<br/>4599 din</a></li>

</ul>

							</div>
						</div>
					</div><!--#ipg_search-->

					<script type=""text/javascript"">
					jQuery(function(){
						jQuery(""#ipg_designs_wide"").easySlider({
							auto: true,
							controlsShow: false,
							pause: 4000,
							continuous: true
						});
					});
					</script>
					
					
					
					<iframe style=""margin-bottom:7px;"" src=""http://www.mojagaraza.rs/widget/?brand=Smart&series=ForFour&model=ForFour&year=2005"" width=""326"" height=""175"" scrolling=""no"" frameborder=""0"" allowtransparency=""true""></iframe>
					

				</div><!--.additional-->
				
				
				
				
				

				

				
				
				<div class=""clearboth""></div>
			</div><!--#tab_bg-->
		</div>

			
			<br/>
			<div class=""ad"" id=""agents-list-view"" style="" border: #EC9D20 1px solid; padding:0px; width:651px; height:90px; background-color:#F6E3E3; margin-bottom:5px; border-radius:7px;-moz-border-radius:7px; -webkit-border-radius:7px;"">
				<script type='text/javascript' src='http://partner.googleadservices.com/gampad/google_service.js'></script>
				<script type='text/javascript'>
				GS_googleAddAdSenseService(""ca-pub-2661848902415312"");
				GS_googleEnableAllServices();
				</script>
				<script type='text/javascript'>
				GA_googleAddSlot(""ca-pub-2661848902415312"", ""polovni_unutar_oglasa"");
				GA_googleAddSlot(""ca-pub-2661848902415312"", ""polovni_unutar_oglasa_levo"");
				</script>
				<script type='text/javascript'>
				GA_googleFetchAds();
				</script>
				
				<div align=""center"" style=""width: 310px; height: 60px; float:left;margin-top:10px;"">
					<!-- ca-pub-2661848902415312/polovni_unutar_oglasa_levo -->
					<script type='text/javascript'>
					GA_googleFillSlot(""polovni_unutar_oglasa_levo"");
					</script>
				</div>
				
				<div align=""center"" style=""width: 310px; height: 60px; float:left;margin-top:10px;"">
					<!-- ca-pub-2661848902415312/polovni_unutar_oglasa -->
					<script type='text/javascript'>
					GA_googleFillSlot(""polovni_unutar_oglasa"");
					</script>
				</div>
			</div><!--#agents-list-view-->
			
			
			
			
		</div>
	</div>
</div>


<div id=""gmap_holder"" class=""jqmWindow-gmap"">
	<div id=""close_btn"" style=""padding-top:10px; float:right;"">
		<a href=""javascript:void(0);"" onclick=""jQuery('#gmap_holder').jqmHide()"" class=""close_btn"">&nbsp;</a></div>
	<div id=""map_canvas""></div>
	<script type=""text/javascript"">
		function initialize() {
			var thisloc = new google.maps.LatLng(45.268520000000000, 19.850828000000000);
			// Detect map type
			var myMapType = locache.get('map_type_id') ? locache.get('map_type_id') : google.maps.MapTypeId.ROADMAP;
			var myOptions = {
				zoom: 15,
				center: thisloc,
				mapTypeId: myMapType
			};
			var map = new google.maps.Map(document.getElementById(""map_canvas""), myOptions);
			var marker = new google.maps.Marker({
				position: thisloc,
				map: map,
				icon: '/images/icons/car-marker.png'
			});
			var infowindow = new google.maps.InfoWindow({
				content: '<dl id=""gmap_dealer_contact"" style=""width:400px"">' + document.getElementById('dealer_contact_map').innerHTML + '</dl>',
				maxWidth: 410
			});
			google.maps.event.addListener(marker, 'click', function() {
				infowindow.open(map, marker);
			});
			// Track map type changes
			google.maps.event.addListener( map, 'maptypeid_changed', function() {
				var type =map.getMapTypeId();
				locache.set('map_type_id', type);
				if (type == ""hybrid"" || type == ""satellite"") {
					_gaq.push(['_trackEvent', 'google-maps', 'click', 'Satellite']);
				} else {
					_gaq.push(['_trackEvent', 'google-maps', 'click', 'Roadmap']);
				}
			} );
		}

		initdone = false;
		function OpenMap(where) {
			if (!initdone) {
				initialize();
				initdone = true;
			}
			_gaq.push(['_trackEvent', 'google-maps', 'click', where]);
			jQuery('#gmap_holder').jqm().jqmShow();
		}
	</script>
</div>


			</div>
		</div>
	</div>
	
	<div id=""banner"">
		<div class=""banner"">
			
			
			
				<div id=""information"" class=""information"">
					<div><h2><a style=""background-position:left; width:130px"" href=""/arhiva-novosti/"">Novosti na sajtu</a></h2></div>
					<div id=""top"">
						


<p>
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	

	

	

	

	

	

	

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	

	
	
	
	
	
	

	
	
	
	
	
	
	
	
		
	
	
	
		    
	
	
	
	
	
	
	
		
	
	
    


	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	

	

	

	

	

	

	

	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	

	
	
	
	
	
	

	
	
	
	
	
	
	
	
		
	
	
	
		    
	
	
	
	
	
	
	
	
		<div id=""featured_articles"">
			<ul class=""extend jcarousel-skin-tango"" id=""novosti-carousel"" >
				
				<li class=""extend"">
					<h3><a href=""/auto-vesti/aktuelno/tv-emisija-abs-show-i-sajt-www-polovniautomobili-com.php"">Video izveštaji sa sajma automobila u Beogradu</a></h3>
					<p>Tv emisija ABS SHOW i sajt www.polovniautomobili.com za Vas će u toku sajma automobila u Beogradu, svakodnevno, počevši od 26.03.2013. pripremati video izveštaje sa promocija novih automobila .Na ...</p>
					<span>18.03.2013. | <a title=""Komentari"" href=""/auto-vesti/aktuelno/tv-emisija-abs-show-i-sajt-www-polovniautomobili-com.php#komentari"" class=""comment"">0</a></span>
				</li>
				
				<li class=""extend"">
					<h3><a href=""/auto-vesti/novosti/dve-nove-opcije-na-sajtu-materijal-enterijera-i-boja.php"">Dve nove opcije na sajtu - materijal enterijera i boja enterijera</a></h3>
					<p>Danas smo uveli dve nove opcije na naš sajt. 
Naime, u kategoriji ""putnička vozila"" uveli smo opcije materijal enterijera i boja enterijera koje za cilj imaju da posetiocima i oglašivačima obezbe...</p>
					<span>15.03.2013. | <a title=""Komentari"" href=""/auto-vesti/novosti/dve-nove-opcije-na-sajtu-materijal-enterijera-i-boja.php#komentari"" class=""comment"">2</a></span>
				</li>
				
				<li class=""extend"">
					<h3><a href=""/auto-vesti/aktuelno/saradnja-sajtova-polovniautomobili-com-i-subotica-com.php"">Saradnja sajtova polovniautomobili.com i subotica.com</a></h3>
					<p>Sajt www.polovniautomobili.com i poznati lokalni sajt www.subotica.com ostvarili su partnersku saradnju. ...</p>
					<span>04.03.2013. | <a title=""Komentari"" href=""/auto-vesti/aktuelno/saradnja-sajtova-polovniautomobili-com-i-subotica-com.php#komentari"" class=""comment"">0</a></span>
				</li>
				
			</ul>
			<script>
			jQuery('#novosti-carousel').jcarousel({vertical: true, visible:1, scroll:1});
			</script>
			
			<div class=""archive-link"" style=""margin-left:100px"">
				[<a href=""/arhiva-novosti/"">Arhiva</a>]
			</div>
			
		</div>
		
	
	
    


	</p>


					</div>
				</div>
			
			
			
			
			<div id=""ssbblock"" align=""center"" style=""padding:10px 0 10px 0; background-image:url(/images/reklama120x600.png); background-repeat:no-repeat; background-position:top;"">
				<div id=""zone7000630"" class=""goAdverticum""></div>
			</div>
			
		</div>
		
		
		
		
			<div id=""sitestats"" style=""width:154px; margin:20px auto;"">
				<div style=""text-align:right;"">Broj aktivnih oglasa: </div>
				<div style=""background-image:url(/images/tablica.png); background-repeat:no-repeat; text-align:center;  width:130px; font-size:22px; padding-left:24px; font-weight:bold; padding-top:5px; padding-bottom:5px;"">92 416</div>

				<div style=""text-align:right;"">Poseta u februaru: </div>
				<div style=""background-image:url(/images/tablica-yellow.png); background-repeat:no-repeat; text-align:center;  width:130px; font-size:22px; padding-left:24px; font-weight:bold; padding-top:5px; padding-bottom:5px;"">6 663 990</div>

			<div style=""text-align:center; color:#999; font-size:11px;"">(izvor: Google Analytics)</div>
			</div>
		
		

	</div>
	
	<div style=""clear: both;""></div>
	<div id=""info"" class=""font11"">
		<p>Copyright &copy; <a href=""http://www.infostud.com"" title=""Infostud - grupa korisnih sajtova"" target=""_blank""><img src=""/images/infostud_logo.png"" alt=""Infostud - grupa korisnih sajtova""  border=""0"" align=""absbottom"" style=""margin: 0 5px -5px 5px""></a> 2000 - 2013.</p> 
		<p><ul>
	<li><a href=""http://www.internet-prodaja-guma.com"" target=""_blank"">internet-prodaja-guma.com</a></li>
	<li><a href=""http://www.mojagaraza.rs/"" target=""_blank"">mojagaraza.rs</a></li>
	<li><a href=""http://poslovi.infostud.com"" target=""_blank"">poslovi.infostud.com</a></li>
	<li><a href=""http://www.mojtim.com"" target=""_blank"">mojtim.com</a></li>
	<li><a href=""http://www.kursevi.com"" target=""_blank"">kursevi.com</a></li>
	<li><a href=""http://www.najstudent.com"" target=""_blank"">najstudent.com</a></li>
	<li><a href=""http://prijemni.infostud.com"" target=""_blank"">prijemni.infostud.com</a></li>
	<li style=""border-right:none""><a href=""http://www.putovanja.info"" target=""_blank"">putovanja.info</a></li>
</ul>
<p>Sadržaj sajta <a href=""http://www.polovniautomobili.com"" style=""color:#b72633"">polovniautomobili.com</a> je vlasništvo <a href=""http://www.infostud.com"" target=""_blank"" style=""color:#46a1e8"">Infostuda</a>. Zabranjeno je njegovo preuzimanje bez dozvole Infostuda, zarad komercijalne upotrebe ili u druge svrhe, osim za lične potrebe posetilaca sajta. <a href=""/wiki/uslovi-koriscenja.php"" style=""font-size:11px; font-weight:normal; color:#46a1e8"">Uslovi korišćenja</a></p></p>
		<p class=""isgroup"">Infostud je deo <a href=""http://www.almamedia.com/"" target=""_blank"" title=""Alma Media"">ALMA</a> grupacije.</p>
	</div>
</div>








<div id=""jqmodal_holder"" class=""jqmWindow"" style=""position: absolute;width:800px;height:735px"">
	<div class=""jqmClose"">
		<div class=""close-bttn""></div>
	</div>
	<div class=""ad-gallery big"">
		<div class=""ad-image-wrapper""></div>
		<div class=""ad-title"">2005 Smart ForFour 1.3</div>
		<div class=""ad-price"">2.850 &euro;</div>
		<div class=""ad-controls""></div>
		<div class=""clear""></div>
		<div class=""ad-nav"">
			<div class=""ad-thumbs"">
				<ul class=""ad-thumb-list"">
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/55bafd711ee7-127x95.jpg"" class=""image0"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/7562aef2731b-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/7562aef2731b-127x95.jpg"" class=""image1"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/3534033ca1e7-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/3534033ca1e7-127x95.jpg"" class=""image2"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/300242bf1af0-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/300242bf1af0-127x95.jpg"" class=""image3"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/fc1097bd5f56-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/fc1097bd5f56-127x95.jpg"" class=""image4"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/9933132ab5af-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/9933132ab5af-127x95.jpg"" class=""image5"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/371d478bb143-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/371d478bb143-127x95.jpg"" class=""image6"">
						</a>
					</li>
					
					<li>
						<a href=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/23e5d53c1f64-800x600.jpg"">
							<img src=""http://images2.polovniautomobili.com/user-images/classifieds/311/3116564/23e5d53c1f64-127x95.jpg"" class=""image7"">
						</a>
					</li>
					
				</ul>
			</div><!--.ad-thumb-list-->
		</div><!--.ad-nav-->
	</div><!--.ad-gallery-->
</div>









	
<div id=""android_banner"" style=""position:fixed;right:0;bottom:0; z-index:999999;"">
	<script type='text/javascript' src='http://partner.googleadservices.com/gampad/google_service.js'>
	</script>
	<script type='text/javascript'>
	GS_googleAddAdSenseService(""ca-pub-2661848902415312"");
	GS_googleEnableAllServices();
	</script>
	<script type='text/javascript'>
	GA_googleAddSlot(""ca-pub-2661848902415312"", ""pa_android_280x126"");
	</script>
	<script type='text/javascript'>
	GA_googleFetchAds();
	</script>
	<script type='text/javascript'>
	   GA_googleFillSlot(""pa_android_280x126"");
	   // Kill the banner after 30sec
	window.setTimeout(
		function() {
			document.getElementById('android_banner').style.display = 'none';
		},
		30000
	);
	</script>
</div>

<!--<script>
	jQuery(window).load(function () {
		var data = {};
		data.url = window.location.href;
		if (jQuery('body').data('ip'))
			{
			data.ip = jQuery('body').data('ip');
			}
		data.load_time = new Date().getTime() - PageTimerStart;

		jQuery.ajax({ url: ""https://api.mongolab.com/api/1/databases/polovni/collections/timing?apiKey=50a8ce09e4b0c737b232abe7"",
			data: JSON.stringify(data),
			type: ""POST"",
			contentType: ""application/json""
			});
		});
</script>-->

<!-- The g3.js should be called once in every page, before the end </body> tag -->
<script type=""text/javascript"" charset=""utf-8"" src=""//ad.adverticum.net/g3.js""></script>
</body>
</html>";
        }
    }

    public class NeuspeloCitanjeStraneException : Exception
    {
        int brojPokusaja;
        public NeuspeloCitanjeStraneException(int brojPokusajaCitanjaStrane)
        {
            brojPokusaja = brojPokusajaCitanjaStrane;
        }
    }
}
