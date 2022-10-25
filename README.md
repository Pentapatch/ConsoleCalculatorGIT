# ConsoleCalculatorGIT
## Betygsgrundande uppgift inom Programmering Grund

---

*Av **Dennis Hankvist**, TUC Yrkeshögskola, Systemutvecklare.NET*
Norrköping/Linköping, HT2022

**Obs! Denna readme är formatterad med GitHub's markdown language, läses bäst på hemsidan.**

---

### Introduktion
Då jag redan besitter ganska goda kunskaper inom programmering så ville jag göra min kalkylator något mer avancerad. 
Att bara kunna välja två tal att addera, subtrahera, multiplicera eller dividera med varandra hade inte tagit så lång tid. 
Jag ville ha lite mer utmaning, så jag satte upp ett mål att kunna skriva in hur många operatorer i ett uttryck man vill. 

Detta löste jag genom att analysera användarens uttryck och extrahera de delar som är intressanta till en lista (av "tokens"). 
Jag går sedan igenom den listan och utför kalkylationen steg för steg. Detta påminner mycket om hur en parser fungerar. 
Men då man måste ta hänsyn till matematiska operatorers prioritetsordning, kände jag att det skulle bli för avancerat 
att skriva en ordentlig parser (som arbetar med tokens bestående av egna klasser).
Därför representeras mina tokens helt enkelt av textsträngar (string) och en ”parser”-metod äger ansvaret att kalkylera dem 
(istället för att kalkylationen görs direkt av en token klass, t.ex. `AdditionToken.Compute()`). 
Validering av tokens sker direkt när de skapas i `ExpressionParser.TokenizeInput()` metoden, och inte av en Lexer metod som en 
ordentlig parser hade använt. Vid sidan av detta projekt har jag dock arbetat på en mer ordentlig parser. 

### Arbetssätt
Att använda GIT var för mig helt nytt och det har varit roligt och bra att lära mig ett nytt källkodshanteringssystem. 
Då jag i början inte var så van vid GIT, arbetade jag i olika projekt för att experimentera lite med koden. 
Halvvägs in på kursen skapade jag en ny repository på GitHub och har sedan dess arbetat med branches för att utveckla 
och redigera projektet. Ibland har jag dock gjort mindre ändringar direkt i master-branchen, då det är ett par extra steg 
för att merge:a ändringar (antar att det är en *bad practice*?). 

Önskar att jag hade den här kunskapen om GIT till mina tidigare, större hobbyprojekt, som till exempel min bildredigerare 
som består av flertalet filer och tusentals rader kod. 

Jag valde snabbt att bryta ner mitt program till olika delar och låta egna klasser ha sina egna ansvarsområden: 
- `Calculator` klassen ansvarar för körning av själva programlogiken (navigering i menysystem, nya beräkningar, visa 
  hjälpavsnitt osv). 
- `IO` klassen ansvarar för all interaktion med `Console` (skriva till konsollen, få tillbaka värden från användaren 
  genom konsollen, skapa och köra menyer i konsollen). 
- `ExpressionParser` klassen ansvarar för att skapa och validera tokens, beräkna uttryck med hjälp av dessa tokens 
  samt formatera uttrycket för bästa läsbarhet. 

Att ha separerat ansvarsområden till klasser gör det lättare att hitta det man söker, och lättare att expandera och 
redigera programmet. Dessa klasser har både publika och privata metoder. 

Anledningen till att jag skapade en egen input/output-klass (`IO`) i stället för att använda `Console.WriteLine()` 
eller `Console.ReadLine()` är egentligen flerfaldig: 
1. Minskar riskerna med att `Console.ForegroundColor` och `Console.BackgroundColor` är *global state* properties. 
2. Gör det lättare att använda färger (endast en rad kod behövs för att skriva i en ny färg, återställs efteråt). 
3. Det låter mig ”highlight:a” {ord} eller {meningar i en annan färg}. 
4. Lättare att konfigurera om ett standard-färgtema om jag vill (genom konstanter som `const DefaultForegroundColor`). 
5. Egna metoder för `IO.GetString()` och `IO.GetDouble()` validerar användarens input direkt och returnerar inte 
   förrän svaret är godtagbart. 
6. Lätt att skapa och köra menyer och undermenyer. 

Det är också en del i min strävan att *alltid* arbeta enligt **DRY principen** (don’t repeat yourself). 
Själva `IO` klassen är den delen av mitt program som består av mest kod, men det är för att jag har planer på att 
fortsätta utveckla det här projektet (*för egen vidareutveckling*) efter att jag lämnat in det. 
Det är också varför jag inte har plockat bort till exempel `IO.GetDouble()` som för tillfället är oanvänd. 
Jag hade kunnat skrivit `IO` klassen som ett *class library* och länkat det till projektet, men då programmet är 
litet kände jag inte ett behov för det. 

Jag har valt att använda mig av ett mycket rakt och enkelt gränssnitt. Förutom välkomstmeddelandet är det för det mesta 
bara text rakt av (om än färgglatt), ingen direkt grafik. Det skulle kunna göras mer estetiskt tilltalande, men jag anser 
att det lätt blir för plottrigt. Mitt program ska vara enkelt att förstå och använda. 
Jag skriver alltid ut vilka knappar som förväntas användas för navigering i programmet. 

Vad gäller **designmönster** så var detta ett nytt koncept för mig. Jag vet att det *alltid finns flera olika sätt* att lösa 
ett problem på. Att det skulle finnas *fastställda lösningar för återkommande problem* är dock något jag antagit. 
Då det här introducerades sent i kursen och jag började arbeta på kalkylatorn i ett tidigt skede, har jag inte följt något 
fastställt designmönster. Det här är något jag kommer att titta djupare på för framtida projekt. 

Jag har sedan tidigare *XML-kommentera* så mycket som möjligt. Då jag har erfarenhet av stora, dåligt dokumenterade 
projekt, vet jag hur viktigt det kan vara. Jag tycker att XML är otroligt hjälpsamt, då det *automatiskt skapar tooltips* i 
visual studio (som visas när man håller muspekaren över den member som blivit kommenterad, eller när man passar in parametrar 
till en metod eller liknande). 

Utöver detta har jag också försökt lägga kommentarer i själva koden. Speciellt på de ställen där det kanske inte är alldeles 
uppenbart vad som utförs. 

Slutligen vill jag nämna att anledningen till att alla klasser är statiska är för att jag inte har något behov av att 
instantiera objekt av dem. Att `Calculator` ligger som en egen klass och inte direkt i *Program.cs*, är för att göra det 
enklare att testköra olika delar av mitt program direkt via top-level statements i `Main()`. 

### FÖRBÄTTRINGSPUNKTER
Jag anser att kod ofta mår bra av att ses över en andra, eller tredje gång under projektets livstid, för att se om det går 
att förenkla den (*refactor*). När jag varit ifrån koden ett tag så inser jag ofta att den är alldeles för rörig, för att 
enkelt kunna överblicka vad den gör. Ett tydligt exempel jag har, är en sats jag hade i `ExpressionParser.ParseExpression()`
metoden: 

`if ((level == 3 && token == "^") || (level == 2 && (token == "*" || token == "/" || token == "%")) || (level == 1 && (token == "+" || token == "-"))) { /* code here */}` 

Allt samlat på en och samma rad, med flera parenteser och **&&** och **||** villkor. Denna sats är numer ersatt med en egen metod: 

`private static bool IsNextOperator(char? mathOperator, int level) => ((level, mathOperator)) switch  
    {  
        (3, ExponentiationOperator) or  
        (2, MultiplicationOperator) or  
        (2, DivisionOperator) or  
        (2, ModulusOperator) or  
        (1, AdditionOperator) or  
        (1, SubtractionOperator) => true,  
        _ => false,  
    };`

(*GitHubs code block ignorerar tydligen whitespace så satsen ovan ser ut att vara på samma rad...*)

Fler rader kod, men för mig betydligt enklare att läsa av. Dessutom är operatörerna jag tittar efter ersatta av konstanter, 
vilket gör det lättare ifall jag i framtiden vill byta ut en symbol för en operatör.

Ifall det här projektet skulle växa skulle det vara bra att göra fler konstanter av värden som kan tänkas upprepas ofta: 

`IO.Wait("Tryck på valfri knapp för att fortsätta...");`

Här skulle textsträngen i argumentet med fördel kunna göras om till `const DefaultWaitMessage`.

Jag skulle också vilja flytta all validering av ett uttryck till en egen metod. I nuläget kontrollerar `ExpressionParser.TokenizeInput()` 
metoden efter alla uttrycksfel utom `DivideByZeroException`, vilket istället testas för i `ExpressionParser.ParseExpression()` metoden. 

Dessutom skickar jag en generell exception (`new Exception(”Error message here”)`) med ett meddelande vid vissa fel. 
Här borde jag istället hitta en mer lämplig exception, eller skapa egna klasser som ärver från `Exception`: 
`class TokenizerException : Exception`. 

Jag skulle också vilja *inkludera mer data* vid en valideringserror, till exempel tokens *index* i input-strängen. Detta så jag 
kan flytta markören i konsollen dit felet påträffats, istället för att användaren måste skriva om hela uttrycket, utan att 
kanske veta vart det blev fel. 

Jag valde att *kasta bort allt oväsentligt* vid konverteringen av input-sträng till tokens. 
Detta kanske borde trigga en varning, då resultatet kanske inte var det som användaren syftade till (i.e. ***1 x 24** kommer tolkas som **124***).

Eftersom det här är ett simpelt console-projekt så har inte *performance* varit något jag behövt tänka så mycket på. 
Tidigare, till exempel i mitt bildredigeringsprogram, har det varit av yttersta vikt att metoderna varit så snabba och 
effektiva som möjligt. Men här spelar det ingen större roll ifall jag optimerat min parser tillräckligt.

### VIDAREUTVECKLING

Jag har *många idéer för vidareutveckling* av det här projektet. Vissa arbetar jag redan på.

Istället för att lagra *uttrycket = resultatet* som en textsträng i historiklistan, skulle jag kunna tänka mig att skapa och 
lagra instanser av en `HistoryEntry` klass med egenskaper som `Expression`, `Result`, `Tokens` och metoder som `ToString()`. 
Det här skulle ge mig fler möjligheter för interaktion med historiken (t.ex. skapa en variabel från ett resultat eller 
infoga uttrycket i nytt uttryck).

En annan fördel med detta är att jag skulle kunna göra *parse*, *tokenize* och *format* metoderna i `ExpressionParser` till 
**private**, och istället lägga till en ny **public** metod `Compute(string expression)` som returnerar ett `HistoryEntry` objekt.

Några fler exempel på vad jag vill göra:

- Tillåta att användaren skapar egna variabler (***X** = 1 + 2 / 3*) som sedan kan användas i nästa beräkning (*2 ^ 5 \* **X***).
- Lösa enkla ekvationer..
- Lägga till matematiska funktioner som *sin*, *cos*, *random*, *round* m.fl.
- Visa steg för steg hur man räknar ut uttrycket *(redan utvecklat, inte integrerat)*.
- Partial evaluation of expression (räkna ut en del av uttrycket för att simplifiera det. *(redan utvecklat, inte integrerat)*).
- Tillåta ändring av uträkningsordning, genom användning av parenteser: *2 ^ (4 - 5)*.
- Meny med inställningar där man bl.a. kan ändra standardfärger, stänga av navigeringshjälp m.m.
- Låta användaren redigera ett uttryck från historiken.
- Låta användaren skapa ett nytt uttryck baserat på resultatet av ett tidigare uttryck från historiken.
- Låta användaren definiera egna matematiska funktioner som sedan kan anropas.
- Lägga in matematiska konstanter som *Pi*, *e*.
- Eventuellt lägga till möjligheten att välja `Decimal` som datatyp, istället för `Double`, vid önskan av mer precision.
