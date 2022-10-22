# ConsoleCalculatorGIT
## Betygsgrundande uppgift inom Programmering Grund

Av **Dennis Hankvist**, TUC Yrkeshögskola, *Systemutvecklare.NET*
Norrköping/Linköping, HT2022

## Introduktion
Anledning till att kalkylatorn ligger i en egen klass i ett eget dokument, för att göra det lättare för mig att
testa olika delar av mitt projekt direkt ifrån Main() i 'Program.cs'. Men jag har även följt principen att lägga
alla klasser i egna filer, för att separera ansvarsområden och göra det lättare att hitta det man söker.
Därför ligger all input/output funktionallitet i 'IO', all övergripande programlogik i 'Calculator' och processering
av matematik-expressions i 'ExpressionParser'. Dessa klasser har både publika och privata metoder.

Jag började med att skriva en egen input/output klass (**IO**) som hjälper mig med nästan all hantering av Console.
Detta dels för att slippa skriva om input-validering överallt där jag behöver be användaren mata in ett värde
(DRY - Don't repeat yourself'), men även för att enkelt kunna skriva ut text i varierande färg (stödjer även
highlighting - genom att omsluta ord eller meningar med {curly braces}).
IO-klassen innehåller även en metod för att skriva ut menyer med custom menyval. Lätt att använda, lätt att återanvända.
Att använda mig av Switch (IO.Menu(...)) reducerar komplexiteten i koden som behöver en meny, vilket ökar läsbarheten.
Att ha gjort en egen metod för Menu() gör det också enkelt att låta menyer ha context-menyer (undermenyer).

Att skriva en simpel kalkylator med addition, subtraktion, multiplikation och division av två tal skulle för mig
inte ta speciellt lång tid, så för att känna att jag kan lära mig något har jag försökt göra den lite mer avancerad.
Min kalkylator kan ta in "obegränsat" antal termer och operatorer i varje uttryck. Detta har jag löst genom att
skapa en enkel variant av en Parser.
Denna parser analyserar användarens input och skapar "tokens" som den sedan använder för att utföra kalkylationen.
Mina tokens är dock enkla text-strängar och inte egna klasser som en riktigt parser hade använt. Jag använder mig heller
inte av en Lexer-metod för att kontrollera syntaxen.

Men när man tillåter expressions med fler än två termer så måste man plötsligt ta hänsyn till "order of operations".
Multiplikation äger större prioritet än addition, medan exponenter går före multiplikation. Detta gör givetvis koden
mer komplicerad, men jag har försökt att kommentera vad som utförs så gott jag kan.

Vid sidan av denna uppgift har jag arbetat på, och exprimenterat lite med fler funktioner till kalkylatorn (bland
annat en ordentlig parser), men jag vill inte lämna in ett allt för stort projekt med exprimentella funktioner,
så jag har hållit dem i separata projekt.

Jag har XML-kommenterat nästan alla metoder, egenskaper och klasser för att göra det enklare för mig, och dig, att
förstå vad de gör. Jag har som vana att alltid kommentera på engelska. Jag tycker inte om när man blandar
svenska och engelska tekniska uttryck.

Jag har använt GIT till detta projektet, något som faktiskt var helt nytt för mig. Lite krångligt i början,
men smidigt med integration i Visual Studio. Det verkar vara ett bra verktyg, något jag skulle haft stor nytta av
i mina tidigare större hobbyprojekt! Nu är det bara jag som har arbetat med detta projektet, så jag har ibland gjort
uppdateringar direkt i master-branchen, men har ofta skapat nya branches när jag ska uppdatera något viktigt, eller
testa en ny funktion.

## Reflektion
Jag har valt att använda mig av ett mycket rakt och enkelt gränsnitt. Förutom välkomstmeddelandet är det för det mesta
bara text rakt av (om än färglatt), ingen direkt grafik. Det skulle kunna göras mer estetiskt tilltalande, men jag anser
att det lätt blir för plottrigt. Mitt program ska vara enkelt att förstå och använda. Jag skriver alltid ut vilka knappar
som ska användas för navigering.

Kod kan i regel alltid förbättras. Jag har insett att det ofta är bra att gå tillbaka till vissa metoder efter en tid,
och se ifall jag kan "refactor:a" den för ökad läsbarhet.

Ta till exempel denna kodbit från metoden ExpressionParser.ParseExpression():
*if ((level == 3 && token == "^") || (level == 2 && (token == "*" || token == "/" || token == "%")) || (level == 1 && (token == "+" || token == "-")))*
Det här satsen kan vara svår att överblicka vad den gör, då allt är samlat på en rad och flera paranterser och && och || används.
Skulle definitivt må bra av lite refactoring.

Eftersom det här är ett simpelt console-projekt så har inte performance varit något jag behövt tänka så mycket på.
Tidigare har jag t.ex. utvecklat ett bildredigeringsprogram, där det har varit av yttersta vikt att metoderna varit
så snabba och effektiva som möjligt. Men här spelar det ingen större roll om min parser gör en onödig iteration i en loop.

Jag skulle kunnat gjort min 'IO' klass som ett class library och refererat till det i det här projektet, men kände att det
blev enklare att inkludera den direkt.

Ifall det här projektet skulle växa skulle det vara bra att göra fler konstanter av värden som kan tänkas upprepas ofta.
Ta exempelvis: *IO.Wait("Tryck på valfri knapp för att fortsätta...");*
Här skulle textsträngen i argumentet med fördel kunna göras om till *const DefaultWaitMessage*.

Nu sparar jag användarens historik som en enkel textsträng som visar *uttrycket = resultatet*. Skulle jag bygga vidare på
projektet så hade jag antagligen skapat en egen klass för resultat med egenskaper för uttryck, resultat och dess tokens.

## Vidareutveckling

Jag har många idéer för vidareutveckling av det här projektet. Vissa arbetar jag redan på.
Då jag har separerat mycket funktionallitet till egna klasser så är det enkelt att bygga vidare.

Några exempel på vad jag vill göra:
- Tillåta användaren att skapa egna variabler (x = 1 + 2 / 3) för att sedan använda dem i nästa beräkning (2 ^ 5 * x).
- Lösa ekvationer..
- Lägga till matematiska funktioner som sin, cos, random, round m.fl.
- Visa steg för steg hur man räknar ut expressionen (redan utvecklat, inte integrerat).
- Partial evaluation av uttrycket (räkna ut en del av uttrycket för att simplifera. Redan utvecklat, inte integrerat).
- Tillåta custom uträkningsordning genom användning av paranteser: 2 ^ (4 - 5).
- Meny med inställningar där man bl.a. kan ändra standardfärger, stänga av navigeringshjälp m.m.
- Låta användaren redigera ett uttryck från historiken.
- Låta användaren definiera egna matematiska funktioner som sedan kan anropas.