# ConsoleCalculatorGIT
## Betygsgrundande uppgift inom Programmering Grund

Av **Dennis Hankvist**, TUC Yrkesh�gskola, *Systemutvecklare.NET*
Norrk�ping/Link�ping, HT2022

## Introduktion
Anledning till att kalkylatorn ligger i en egen klass i ett eget dokument, f�r att g�ra det l�ttare f�r mig att
testa olika delar av mitt projekt direkt ifr�n Main() i 'Program.cs'. Men jag har �ven f�ljt principen att l�gga
alla klasser i egna filer, f�r att separera ansvarsomr�den och g�ra det l�ttare att hitta det man s�ker.
D�rf�r ligger all input/output funktionallitet i 'IO', all �vergripande programlogik i 'Calculator' och processering
av matematik-expressions i 'ExpressionParser'. Dessa klasser har b�de publika och privata metoder.

Jag b�rjade med att skriva en egen input/output klass (**IO**) som hj�lper mig med n�stan all hantering av Console.
Detta dels f�r att slippa skriva om input-validering �verallt d�r jag beh�ver be anv�ndaren mata in ett v�rde
(DRY - Don't repeat yourself'), men �ven f�r att enkelt kunna skriva ut text i varierande f�rg (st�djer �ven
highlighting - genom att omsluta ord eller meningar med {curly braces}).
IO-klassen inneh�ller �ven en metod f�r att skriva ut menyer med custom menyval. L�tt att anv�nda, l�tt att �teranv�nda.
Att anv�nda mig av Switch (IO.Menu(...)) reducerar komplexiteten i koden som beh�ver en meny, vilket �kar l�sbarheten.
Att ha gjort en egen metod f�r Menu() g�r det ocks� enkelt att l�ta menyer ha context-menyer (undermenyer).

Att skriva en simpel kalkylator med addition, subtraktion, multiplikation och division av tv� tal skulle f�r mig
inte ta speciellt l�ng tid, s� f�r att k�nna att jag kan l�ra mig n�got har jag f�rs�kt g�ra den lite mer avancerad.
Min kalkylator kan ta in "obegr�nsat" antal termer och operatorer i varje uttryck. Detta har jag l�st genom att
skapa en enkel variant av en Parser.
Denna parser analyserar anv�ndarens input och skapar "tokens" som den sedan anv�nder f�r att utf�ra kalkylationen.
Mina tokens �r dock enkla text-str�ngar och inte egna klasser som en riktigt parser hade anv�nt. Jag anv�nder mig heller
inte av en Lexer-metod f�r att kontrollera syntaxen.

Men n�r man till�ter expressions med fler �n tv� termer s� m�ste man pl�tsligt ta h�nsyn till "order of operations".
Multiplikation �ger st�rre prioritet �n addition, medan exponenter g�r f�re multiplikation. Detta g�r givetvis koden
mer komplicerad, men jag har f�rs�kt att kommentera vad som utf�rs s� gott jag kan.

Vid sidan av denna uppgift har jag arbetat p�, och exprimenterat lite med fler funktioner till kalkylatorn (bland
annat en ordentlig parser), men jag vill inte l�mna in ett allt f�r stort projekt med exprimentella funktioner,
s� jag har h�llit dem i separata projekt.

Jag har XML-kommenterat n�stan alla metoder, egenskaper och klasser f�r att g�ra det enklare f�r mig, och dig, att
f�rst� vad de g�r. Jag har som vana att alltid kommentera p� engelska. Jag tycker inte om n�r man blandar
svenska och engelska tekniska uttryck.

Jag har anv�nt GIT till detta projektet, n�got som faktiskt var helt nytt f�r mig. Lite kr�ngligt i b�rjan,
men smidigt med integration i Visual Studio. Det verkar vara ett bra verktyg, n�got jag skulle haft stor nytta av
i mina tidigare st�rre hobbyprojekt! Nu �r det bara jag som har arbetat med detta projektet, s� jag har ibland gjort
uppdateringar direkt i master-branchen, men har ofta skapat nya branches n�r jag ska uppdatera n�got viktigt, eller
testa en ny funktion.

## Reflektion
Jag har valt att anv�nda mig av ett mycket rakt och enkelt gr�nsnitt. F�rutom v�lkomstmeddelandet �r det f�r det mesta
bara text rakt av (om �n f�rglatt), ingen direkt grafik. Det skulle kunna g�ras mer estetiskt tilltalande, men jag anser
att det l�tt blir f�r plottrigt. Mitt program ska vara enkelt att f�rst� och anv�nda. Jag skriver alltid ut vilka knappar
som ska anv�ndas f�r navigering.

Kod kan i regel alltid f�rb�ttras. Jag har insett att det ofta �r bra att g� tillbaka till vissa metoder efter en tid,
och se ifall jag kan "refactor:a" den f�r �kad l�sbarhet.

Ta till exempel denna kodbit fr�n metoden ExpressionParser.ParseExpression():
*if ((level == 3 && token == "^") || (level == 2 && (token == "*" || token == "/" || token == "%")) || (level == 1 && (token == "+" || token == "-")))*
Det h�r satsen kan vara sv�r att �verblicka vad den g�r, d� allt �r samlat p� en rad och flera paranterser och && och || anv�nds.
Skulle definitivt m� bra av lite refactoring.

Eftersom det h�r �r ett simpelt console-projekt s� har inte performance varit n�got jag beh�vt t�nka s� mycket p�.
Tidigare har jag t.ex. utvecklat ett bildredigeringsprogram, d�r det har varit av yttersta vikt att metoderna varit
s� snabba och effektiva som m�jligt. Men h�r spelar det ingen st�rre roll om min parser g�r en on�dig iteration i en loop.

Jag skulle kunnat gjort min 'IO' klass som ett class library och refererat till det i det h�r projektet, men k�nde att det
blev enklare att inkludera den direkt.

Ifall det h�r projektet skulle v�xa skulle det vara bra att g�ra fler konstanter av v�rden som kan t�nkas upprepas ofta.
Ta exempelvis: *IO.Wait("Tryck p� valfri knapp f�r att forts�tta...");*
H�r skulle textstr�ngen i argumentet med f�rdel kunna g�ras om till *const DefaultWaitMessage*.

Nu sparar jag anv�ndarens historik som en enkel textstr�ng som visar *uttrycket = resultatet*. Skulle jag bygga vidare p�
projektet s� hade jag antagligen skapat en egen klass f�r resultat med egenskaper f�r uttryck, resultat och dess tokens.

## Vidareutveckling

Jag har m�nga id�er f�r vidareutveckling av det h�r projektet. Vissa arbetar jag redan p�.
D� jag har separerat mycket funktionallitet till egna klasser s� �r det enkelt att bygga vidare.

N�gra exempel p� vad jag vill g�ra:
- Till�ta anv�ndaren att skapa egna variabler (x = 1 + 2 / 3) f�r att sedan anv�nda dem i n�sta ber�kning (2 ^ 5 * x).
- L�sa ekvationer..
- L�gga till matematiska funktioner som sin, cos, random, round m.fl.
- Visa steg f�r steg hur man r�knar ut expressionen (redan utvecklat, inte integrerat).
- Partial evaluation av uttrycket (r�kna ut en del av uttrycket f�r att simplifera. Redan utvecklat, inte integrerat).
- Till�ta custom utr�kningsordning genom anv�ndning av paranteser: 2 ^ (4 - 5).
- Meny med inst�llningar d�r man bl.a. kan �ndra standardf�rger, st�nga av navigeringshj�lp m.m.
- L�ta anv�ndaren redigera ett uttryck fr�n historiken.
- L�ta anv�ndaren definiera egna matematiska funktioner som sedan kan anropas.