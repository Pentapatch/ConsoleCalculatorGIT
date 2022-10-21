# ConsoleCalculatorGIT
## Betygsgrundande uppgift inom Programmering Grund

Av **Dennis Hankvist**, TUC Yrkeshögskola, *Systemutvecklare.NET*
Norrköping/Linköping, HT2022

## Introduktion
Anledning till att kalkylatorn ligger i en egen klass i ett eget dokument, är för att göra det lättare för mig att
testa olika delar av mitt projekt direkt ifrån Main() i 'Program.cs'.

Jag började med att skriva en egen input/output klass (**IO**) som hjälper mig med nästan all hantering av Console.
Detta dels för att slippa skriva om input-validering överallt där jag behöver be användaren mata in ett värde
(DRY - Don't repeat yourself'), men även för att enkelt kunna skriva ut text i varierande färg (stödjer även
highlighting - genom att omsluta ord eller meningar med {curly braces}).
IO-klassen innehåller även en metod för att skriva ut menyer med custom menyval. Lätt att använda, lätt att återanvända.
Att använda mig av Switch (IO.Menu(...)) reducerar komplexiteten i koden vilket ökar läsbarheten.

Att skriva en simpel kalkylator med addition, subtraktion, multiplikation och division av två tal skulle för mig inte ta lång tid,
så jag har istället lagt ner lite extra tid på att få den något mer avancerad. Min kalkylator kan ta in "obegränsat" antal
termer och operatorer i varje expression. Detta har jag löst genom att skapa en enkel variant av en Parser.
Denna parser analyserar användarens input och skapar "Tokens" som den sedan använder för att utföra kalkylationen.
Mina tokens är dock enkla text-strängar och inte egna klasser som en riktigt parser hade använt. Jag använder mig heller
inte av en Lexer-metod för att kontrollera syntaxen.

Vid sidan av denna uppgift har jag arbetat och exprimenterat med lite med fler funktioner till kalkylatorn,
men jag vill inte lämna ett allt för stort projekt.

Jag har XML-kommenterat nästan alla methods och properties.

Jag har använt GIT till detta projektet, något som faktiskt var helt nytt för mig.

## Förbättringspunkter