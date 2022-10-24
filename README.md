# ConsoleCalculatorGIT
## Betygsgrundande uppgift inom Programmering Grund

Av **Dennis Hankvist**, TUC Yrkesh�gskola, *Systemutvecklare.NET*
Norrk�ping/Link�ping, HT2022

Obs: Denna readme �r formatterad med GitHub's markdown language, l�ses b�st p� hemsidan.

### Introduktion
D� jag redan besitter ganska goda kunskaper inom programmering s� ville jag g�ra min kalkylator n�got mer avancerad. 
Att bara kunna v�lja tv� tal att addera, subtrahera, multiplicera eller dividera med varandra hade inte tagit s� l�ng tid. 
Jag ville ha lite mer utmaning, s� jag satte upp ett m�l att kunna skriva in hur m�nga operatorer i ett uttryck man vill. 

Detta l�ste jag genom att analysera anv�ndarens uttryck och extrahera de delar som �r intressanta till en lista (av "tokens"). 
Jag g�r sedan igenom den listan och utf�r kalkylationen steg f�r steg. Detta p�minner mycket om hur en parser fungerar. 
Men d� man m�ste ta h�nsyn till matematiska operatorers prioritetsordning, k�nde jag att det skulle bli f�r avancerat 
att skriva en ordentlig parser (som arbetar med tokens best�ende av egna klasser).
D�rf�r representeras mina tokens helt enkelt av textstr�ngar (string) och en �parser�-metod �ger ansvaret att kalkylera dem 
(ist�llet f�r att kalkylationen g�rs direkt av en token klass, t.ex. ```AdditionToken.Compute()```). 
Validering av tokens sker direkt n�r de skapas i ```TokenizeInput()``` metoden, och inte av en Lexer metod som en ordentlig 
parser hade anv�nt. Vid sidan av detta projekt har jag dock arbetat p� en mer ordentlig parser. 

### Arbetss�tt
Att anv�nda GIT var f�r mig helt nytt och det har varit roligt och bra att l�ra mig ett nytt k�llkodshanteringssystem. 
D� jag i b�rjan inte var s� van vid GIT, arbetade jag i olika projekt f�r att experimentera lite med koden. 
Halvv�gs in p� kursen skapade jag en ny repository p� GitHub och har sedan dess arbetat med branches f�r att utveckla 
och redigera projektet. Ibland har jag dock gjort mindre �ndringar direkt i master-branchen, d� det �r ett par extra steg 
f�r att merge:a �ndringar (antar att det �r en bad practice?). 

�nskar att jag hade den h�r kunskapen om GIT till mina tidigare, st�rre hobbyprojekt, som till exempel min bildredigerare 
som best�r av flertalet filer och tusentals rader kod. 

Jag valde snabbt att bryta ner mitt program till olika delar och l�ta egna klasser ha sina egna ansvarsomr�den: 
- **Calculator** klassen ansvarar f�r k�rning av sj�lva programlogiken (navigering i menysystem, nya ber�kningar, visa 
  hj�lpavsnitt osv). 
- **IO** klassen ansvarar f�r all interaktion med ```Console``` (skriva till konsollen, f� tillbaka v�rden fr�n anv�ndaren 
  genom konsollen, skapa och k�ra menyer i konsollen). 
- **ExpressionParser** klassen ansvarar f�r att skapa och validera tokens, ber�kna uttryck med hj�lp av dessa tokens 
  samt formatera uttrycket f�r b�sta l�sbarhet. 

Att ha separerat ansvarsomr�den till klasser g�r det l�ttare att hitta det man s�ker, och l�ttare att expandera och 
redigera programmet. Dessa klasser har b�de publika och privata metoder. 

Anledningen till att jag skapade en egen input/output-klass (**IO**) i st�llet f�r att anv�nda ```Console.WriteLine()``` 
eller ```Console.ReadLine()``` �r egentligen flerfaldig: 
1. Minskar riskerna med att ```Console.ForegroundColor``` och ```Console.BackgroundColor``` �r *global state* properties. 
2. G�r det l�ttare att anv�nda f�rger (endast en rad kod beh�vs f�r att skriva i en ny f�rg). 
3. Det l�ter mig �highlight:a� ord eller meningar i en annan f�rg. 
4. L�ttare att konfigurera om ett standard-f�rgtema om jag vill (genom konstanter som ```const DefaultForegroundColor```). 
5. Egna metoder f�r ```IO.GetString()``` och ```IO.GetDouble()``` validerar anv�ndarens input direkt och returnerar inte 
   f�rr�n svaret �r godtagbart. 
6. L�tt att skapa och k�ra menyer och undermenyer. 

Det �r ocks� en del i min str�van att *alltid* arbeta enligt DRY principen (don�t repeat yourself). 
Sj�lva **IO** klassen �r den delen av mitt program som best�r av mest kod, men det �r f�r att jag har planer p� att 
forts�tta utveckla det h�r projektet (f�r egen vidareutveckling) efter att jag l�mnat in det. 
Det �r ocks� varf�r jag inte har plockat bort till exempel ```IO.GetDouble()``` som f�r tillf�llet �r oanv�nd. 
Jag hade kunnat skrivit **IO** klassen som ett *class library* och l�nkat det till projektet, men d� programmet �r 
litet k�nde jag inte ett behov f�r det. 

Jag har valt att anv�nda mig av ett mycket rakt och enkelt gr�nssnitt. F�rutom v�lkomstmeddelandet �r det f�r det mesta 
bara text rakt av (om �n f�rgglatt), ingen direkt grafik. Det skulle kunna g�ras mer estetiskt tilltalande, men jag anser 
att det l�tt blir f�r plottrigt. Mitt program ska vara enkelt att f�rst� och anv�nda. 
Jag skriver alltid ut vilka knappar som f�rv�ntas anv�ndas f�r navigering i programmet. 

Vad g�ller *designm�nster* s� var detta ett nytt koncept f�r mig. Jag vet att det alltid finns flera olika s�tt att l�sa 
ett problem p�. Att det skulle finnas fastst�llda l�sningar f�r �terkommande problem �r dock n�got jag antagit. 
D� detta kom sent i kursen och jag b�rjade arbeta p� kalkylatorn i ett tidigt skede, har jag inte f�ljt n�got fastst�llt 
designm�nster. Det h�r �r n�got jag kommer att titta djupare p�. 

Jag har sedan tidigare b�rjat *XML-kommentera* s� mycket som m�jligt. D� jag har erfarenhet av stora, d�ligt dokumenterade 
projekt, vet jag hur viktigt det kan vara. Jag tycker att XML �r otroligt hj�lpsamt, d� det automatiskt skapar tooltips i 
visual studio (som visas n�r man h�ller muspekaren �ver den member som blivit kommenterad, eller n�r man passar in parametrar 
till en metod eller liknande). 

Ut�ver detta har jag ocks� f�rs�kt l�gga kommentarer i sj�lva koden. Speciellt p� de st�llen d�r det kanske inte �r alldeles 
uppenbart vad som utf�rs. 

Slutligen vill jag n�mna att anledningen till att alla klasser �r statiska �r f�r att jag inte har n�got behov av att 
instantiera objekt av dem. Att **Calculator** ligger som en egen klass och inte direkt i *Program.cs*, �r f�r att g�ra det 
enklare att testk�ra olika delar av mitt program direkt via top-level statements i ```Main()```. 

### F�RB�TTRINGSPUNKTER
Jag anser att kod ofta m�r bra av att ses �ver en andra, eller tredje g�ng under projektets livstid, f�r att se om det g�r 
att f�renkla den (*refactor*). N�r jag varit ifr�n koden ett tag s� inser jag ofta att den �r alldeles f�r r�rig, f�r att 
enkelt kunna �verblicka vad den g�r. Ett tydligt exempel jag har, �r en sats jag hade i ```ExpressionParser.ParseExpression()``` 
metoden: 

```if ((level == 3 && token == "^") || (level == 2 && (token == "*" || token == "/" || token == "%")) || (level == 1 && (token == "+" || token == "-"))) { /* code here */}``` 

Allt samlat p� en och samma rad, med flera parenteser och **&&** och **||** villkor. Denna sats �r numer ersatt med en egen metod: 

```private static bool IsNextOperator(char? mathOperator, int level) => ((level, mathOperator)) switch
        {
            (3, ExponentiationOperator) or
            (2, MultiplicationOperator) or
            (2, DivisionOperator) or
            (2, ModulusOperator) or
            (1, AdditionOperator) or
            (1, SubtractionOperator) => true,
            _ => false,
        };```

Fler rader kod, men f�r mig betydligt enklare att l�sa av. Dessutom �r operat�rerna jag tittar efter ersatta av konstanter, 
vilket g�r det l�ttare ifall jag i framtiden vill byta ut en symbol f�r en operat�r.

Ifall det h�r projektet skulle v�xa skulle det vara bra att g�ra fler konstanter av v�rden som kan t�nkas upprepas ofta: 

```IO.Wait("Tryck p� valfri knapp f�r att forts�tta...");```

H�r skulle textstr�ngen i argumentet med f�rdel kunna g�ras om till ```const DefaultWaitMessage = "Tryck p� valfri knapp f�r att forts�tta...";```.

Jag skulle ocks� vilja flytta all validering av ett uttryck till en egen metod. I nul�get kontrollerar ```ExpressionParser.TokenizeInput()``` 
metoden efter alla uttrycksfel utom ```DivideByZeroException```, vilket ist�llet testas f�r i ```ExpressionParser.ParseExpression()``` metoden. 
Dessutom skickar jag en generell exception (```new Exception(�Error message here�)```) med ett meddelande vid vissa fel. 
H�r borde jag ist�llet hitta en mer l�mplig exception, eller skapa egna klasser som �rver fr�n ```Exception```: 
```class OperatorException : Exception```. 

Jag skulle ocks� vilja inkludera mer data vid en valideringserror, till exempel tokens index i input-str�ngen. Detta s� jag 
kan flytta mark�ren i konsollen dit felet p�tr�ffats, ist�llet f�r att anv�ndaren m�ste skriva om hela uttrycket, utan att 
kanske veta vart det blev fel. 

Jag valde ocks� att kasta bort allt ov�sentligt vid konverteringen av en input-str�ng till tokens. 
Detta kanske borde trigga en varning, d� resultatet kanske inte var det som anv�ndaren syftade till (*1 x 24 kommer tolkas som 124*).

Eftersom det h�r �r ett simpelt console-projekt s� har inte *performance* varit n�got jag beh�vt t�nka s� mycket p�. 
Tidigare, till exempel i mitt bildredigeringsprogram, har det varit av yttersta vikt att metoderna varit s� snabba och 
effektiva som m�jligt. Men h�r spelar det ingen st�rre roll om jag optimerat min parser tillr�ckligt.

### VIDAREUTVECKLING

Jag har m�nga id�er f�r vidareutveckling av det h�r projektet. Vissa arbetar jag redan p�.

N�gra exempel p� vad jag vill g�ra:
- Till�ta att anv�ndaren skapar egna variabler (*x = 1 + 2 / 3*) som sedan kan anv�ndas i n�sta ber�kning (*2 ^ 5 * x*).
- L�sa enkla ekvationer..
- L�gga till matematiska funktioner som sin, cos, random, round m.fl.
- Visa steg f�r steg hur man r�knar ut uttrycket (redan utvecklat, inte integrerat).
- Partial evaluation of expression (r�kna ut en del av uttrycket f�r att simplifiera det. (redan utvecklat, inte integrerat).
- Till�ta �ndring av utr�kningsordning, genom anv�ndning av parenteser: *2 ^ (4 - 5)*.
- Meny med inst�llningar d�r man bl.a. kan �ndra standardf�rger, st�nga av navigeringshj�lp m.m.
- L�ta anv�ndaren redigera ett uttryck fr�n historiken.
- L�ta anv�ndaren skapa ett nytt uttryck baserat p� resultatet av ett tidigare uttryck fr�n historiken.
- L�ta anv�ndaren definiera egna matematiska funktioner som sedan kan anropas.
- L�gga in matematiska konstanter som Pi, e.
- Eventuellt l�gga till m�jligheten att v�lja decimal som datatyp, ist�llet f�r double, vid �nskan av mer precision.

