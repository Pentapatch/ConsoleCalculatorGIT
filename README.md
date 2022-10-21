# ConsoleCalculatorGIT
## Betygsgrundande uppgift inom Programmering Grund

Av **Dennis Hankvist**, TUC Yrkesh�gskola, *Systemutvecklare.NET*
Norrk�ping/Link�ping, HT2022

## Introduktion
Anledning till att kalkylatorn ligger i en egen klass i ett eget dokument, �r f�r att g�ra det l�ttare f�r mig att
testa olika delar av mitt projekt direkt ifr�n Main() i 'Program.cs'.

Jag b�rjade med att skriva en egen input/output klass (**IO**) som hj�lper mig med n�stan all hantering av Console.
Detta dels f�r att slippa skriva om input-validering �verallt d�r jag beh�ver be anv�ndaren mata in ett v�rde
(DRY - Don't repeat yourself'), men �ven f�r att enkelt kunna skriva ut text i varierande f�rg (st�djer �ven
highlighting - genom att omsluta ord eller meningar med {curly braces}).
IO-klassen inneh�ller �ven en metod f�r att skriva ut menyer med custom menyval. L�tt att anv�nda, l�tt att �teranv�nda.
Att anv�nda mig av Switch (IO.Menu(...)) reducerar komplexiteten i koden vilket �kar l�sbarheten.

Att skriva en simpel kalkylator med addition, subtraktion, multiplikation och division av tv� tal skulle f�r mig inte ta l�ng tid,
s� jag har ist�llet lagt ner lite extra tid p� att f� den n�got mer avancerad. Min kalkylator kan ta in "obegr�nsat" antal
termer och operatorer i varje expression. Detta har jag l�st genom att skapa en enkel variant av en Parser.
Denna parser analyserar anv�ndarens input och skapar "Tokens" som den sedan anv�nder f�r att utf�ra kalkylationen.
Mina tokens �r dock enkla text-str�ngar och inte egna klasser som en riktigt parser hade anv�nt. Jag anv�nder mig heller
inte av en Lexer-metod f�r att kontrollera syntaxen.

Vid sidan av denna uppgift har jag arbetat och exprimenterat med lite med fler funktioner till kalkylatorn,
men jag vill inte l�mna ett allt f�r stort projekt.

Jag har XML-kommenterat n�stan alla methods och properties.

Jag har anv�nt GIT till detta projektet, n�got som faktiskt var helt nytt f�r mig.

## F�rb�ttringspunkter