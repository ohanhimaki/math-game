# Spotify quiz game improvements, vaihe 1

## Kielisyys

Näytöllä näkyy usealla eri kielellä tekstejä, korjataan niin että vain
kertaalleen tekstit.

## Omacsv ohjeet 

Lisätään Lataa oma csv painikkeen viereen info painike josta aukeaa modali, joka
sisältää ohjeet oman csv:n muodostukseen: 
Jos ohjeita täällä ei vielä ole niin menee karkeasti: 

Spotifyssä sopiva soittolista: huomiona kannattaa pitää että käytetään
automaattisesti näiden kappaleiden spotifystä löytyvää julkaisuvuotta, eli
bestof levyt ym. aiheuttaa ongelmia. 

Exportataan soittolista täällä:
https://www.chosic.com/spotify-playlist-exporter/

Tämä csv on sitten kelpoinnen tähän importattavaksi.


## teksti virhe: Vuosikymmenarvaus kuin vai 1 kortti

Vuosikymmenarvaus kuin vai 1 kortti -> Pitäis olla mahdollisesti kun 0 korttia.
(mielestäni toiminnallisuus on oikein, ainoastaan tämä teksti väärin)

## Säännöt

Initial cards kuuluu pelisääntöjen puolelle, nyt se on ennen sitä otsikkoa. 
Sen lisäksi laitetaan defaultit: 
0 korttia, 
vuosikymmenarvaus kun 0 korttia, 
ei ryöstökortteja 
(ryöstökortit boolean sais mahdollisesti merkitä nämä muut
ryöstökortti-asetukset disablediksi koska niillä ei pitäisi olla vaikutuksia)

# vaihe 2 

## Vuosikymmen pikanäppäimet 

vuosikymmen arvauksessa olisi kiva että pystyisi nuolilla kohdistamaan ja
spacella valitsemaan 


