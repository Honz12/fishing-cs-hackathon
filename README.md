# Kde Jsou Ryby?!

Konzolová textová rybářská hra v C# s barevnou grafikou díky ANSI escape sekvencím. Chytej ryby, vylepšuj své vybavení a staň se nejlepším rybářem!

## O hře

Hráč se ocitá v roli rybáře, který chytá ryby pomocí minihry s pohybujícím se kurzorem. Chycené ryby se zobrazují jako 16×16 pixelové ASCII obrázky přímo v terminálu.

Hra obsahuje **20+ druhů ryb** — 10 sladkovodních a 10 mořských + tajné ryby, každá s vlastní vahou, raritou a požadovanou úrovní prutu.

## Tutoriál
[Přejít tutoriál](TUTORIAL.md)

### Herní smyčka

1. **Hlavní Menu** — možnost chytat ryby, navštívit obchod nebo odejít
2. **Chytání Ryb** — minihra s pohybující se značkou; načasování určí, zda rybu chytíš
3. **Obchod** — nakup vylepšení prutu (odemyká možnost chytit vzácnější ryby) a lodě (větší Chladící Box)
4. **Chladící Box** — přehled všech chycených ryb

## Jak spustit

```bash
dotnet run
```

## Autoři

| Jméno | Co dělal |
|---|---|
| **Honz12** | Hlavní herní smyčka, minihra chytání, datový model ryb, systém ANSI obrázků, boot screen |
| **Matěj Albert** | Obchod (vylepšení prutu/lodě), úpravy menu, README & tutoriál, Testování |
| **sebastianjecny-green** | pixel art ryb, prutů a lodí |
