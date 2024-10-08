# HrApp

Ohjelman tarkoituksena on toimia sovelluksena, jolla voidaan hallita yrityksen henkilötietoja sekä seurata henkilöstön työsuhteita. Sovellus koostuu kahdesta pääsivusta: kirjautumissivusta ja pääsivusta.

Kirjautumissivu on suunniteltu käyttäjän tunnistautumiseen. Lisäksi kirjautumissivulla on mahdollisuus lisätä uusi käyttäjä painamalla salasanakentän alapuolella olevaa painiketta.

Pääsivu sisältää osiot henkilöiden ja heidän työsuhteidensa hallintaan. Sovelluksen kautta käyttäjä voi lisätä, muokata, tarkastella ja poistaa henkilötietoja sekä työsuhteisiin liittyviä tietoja.

Sovelluksessa on useita lisäominaisuuksia:

- Henkilöiden ja heidän työsuhteidensa tiedot salataan ennen tallennusta tekstitiedostoihin.
- Kaikki tapahtumat tallennetaan lokitiedostoon.
- Sovellus osaa ehdottaa kaupunkeja syötetyn postinumeron perusteella.
- Sovellus tarkistaa, että syötetty henkilötunnus on oikeassa muodossa.
- Sovellus on toteutettu Visual Studiolla käyttäen .NET MAUI -teknologiaa. Ohjelmointikielinä on käytetty C# ja XAML.

# Käyttöohje
Tämä sovellus käyttää C://temp/Hrfiles-kansiota tiedostojensa tallentamiseen. Ohjelma luo Hrfiles-kansion ja kaikki tarvittavat tekstitiedostot automaattisesti, mutta C://temp/-kansion olemassaolo on edellytys ohjelman toiminnalle. Jos C://temp/-kansio on olemassa ja sinne pääsy on esteetön, tarvittavat tiedostot ja kansiot luodaan automaattisesti.

Kirjautumissivu
Kun sovellus avataan, ensimmäisenä näkyy kirjautumissivu (Kuva 1). Kirjautumissivulle syötetään käyttäjätunnus ja salasana. Jos haluat luoda uuden käyttäjän, paina "Rekisteröidy uudella käyttäjällä" -painiketta. Tämän jälkeen avautuu näkymä, jossa voit luoda uuden käyttäjän (Kuva 2). Onnistuneen käyttäjän luonnin jälkeen ohjelma ilmoittaa onnistumisesta (Kuva 3). Jos käyttäjätunnus on jo olemassa, saat ilmoituksen siitä.

Ohjelmassa on oletuksena admin-käyttäjä, joten erillistä käyttäjän luonnin tarvetta ei välttämättä ole.

- Käyttäjätunnus: admin
- Salasana: admin
Pääsivu
Onnistuneen kirjautumisen jälkeen avautuu pääsivu (Kuva 4). Sivun yläkulmassa näkyy aktiivinen käyttäjä tai valittu henkilö. Tämän valinnan perusteella määritetään, mitä toimintoja käyttäjä voi suorittaa. Esimerkiksi, kun uusi käyttäjä on valittuna, voit lisätä uusia henkilöitä. Jos valitset listalta tietyn henkilön, voit muokata tai tarkastella tämän tietoja (Kuva 5). Vanhoja tietoja voi muokata tai poistaa. Poistamisen yhteydessä ohjelma varmistaa päätöksesi (Kuva 6). Henkilön valinnan voi peruuttaa painamalla "Uusi"-painiketta tai valitsemalla toisen henkilön listalta.

Sovellus tarjoaa myös ehdotuksia postitoimipaikoille syötetyn postinumeron perusteella (Kuva 7). Henkilötunnuksen on oltava oikeassa muodossa, muuten ohjelma ei salli tietojen lisäämistä tai muokkaamista (Kuva 8).

Toimisuhde-osio
Toimisuhde-osio tulee käyttöön, kun tietty henkilö on valittuna. Toiminta on lähes identtinen henkilötieto-osion kanssa: napit ja toimisuhteen valintaperiaatteet ovat samankaltaisia (Kuva 9). Toimisuhde voi olla toistaiseksi voimassa oleva, ja tätä määrittää nappi "Loppumispäivä"-kentän alla.

Lokitiedot
Sovellus sisältää myös lokitiedot, jotka tallennetaan tiedostoon C:\Temp\HrFiles\log.json. Lokitiedot ovat JSON-muodossa ja sisältävät tiedot siitä, kuka teki, mitä teki ja milloin teki.

![image](https://github.com/user-attachments/assets/f01c9b63-41eb-43a5-826f-4c7cc5332b5c)
Kuva 1. 

![image](https://github.com/user-attachments/assets/5acbd797-728f-4cc8-9563-162aa1f7d520)
Kuva 2.

![image](https://github.com/user-attachments/assets/a4725a5c-5ed4-4576-9cd2-c2e9193a98e8)
Kuva 3.

![image](https://github.com/user-attachments/assets/fdd67204-2f48-4b26-b85d-812a4c45fae2)
Kuva 4.

![image](https://github.com/user-attachments/assets/030c4b8f-c78e-461c-8639-872c3e02f788)
Kuva 5.

![image](https://github.com/user-attachments/assets/d557c393-4c17-4712-91ea-563f0942aabb)
Kuva 6.

![image](https://github.com/user-attachments/assets/413be9c4-daef-4e1a-93ad-07b993175064)
Kuva 7. 

![image](https://github.com/user-attachments/assets/bf044364-b6c0-48e4-8ed7-4e0a7e43d722)
Kuva 8.

![image](https://github.com/user-attachments/assets/2ba3dfbe-32c4-47c7-9dba-ae817ce8fd3b)
kuva 9.

Liittyvät tiedostot
Sovellus käyttää C://temp/Hrfiles-kansiota liittyville tiedostoilleen. Tämä kansio sisältää loki-, henkilö- ja toimisuhdetiedot sekä käyttäjätiedot. Kansio ja siihen kuuluvat tiedostot luodaan automaattisesti, kunhan temp-kansio on olemassa C-asemalla.

Tiedostot
- users.json
  Tämä tiedosto sisältää kirjautumistiedot sovelluksen käyttäjille. Huomioi, että käyttäjätiedot eivät liity sovelluksen käsittelemiin henkilötietoihin, vaan ne ovat ainoastaan kirjautumista varten. Tämä tiedosto ei ole     salattu.

- log.json
  Tämä tiedosto sisältää sovelluksen lokitiedot JSON-muodossa. Lokitiedoista voi nähdä, mitä käyttäjät ovat tehneet ja milloin. Tämä tiedosto ei ole salattu.

- employ.txt
Tämä tiedosto sisältää henkilötiedot ja toimisuhdetiedot. Tiedoston sisältö on JSON-merkkijono, joka on salattu kovakoodatulla avaimella. Jos tiedoston avaa ilman purkamista, sen sisältö näyttää satunnaiselta         numerosarjalta ja kirjaimilta (Kuva 10).

![image](https://github.com/user-attachments/assets/cf1e3f3b-edd4-44ae-bed3-c502050f216d)
Kuva 10.
