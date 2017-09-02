# MaterialPalette
Replicates the Android palette system for Rainmeter.

Basic planned functionallity is to have 2 different types on info: Swatches and Palettes. Which can be generated a variety of ways 
Swatches: Google's color swatches, can use the pregenerated ones or generate it from a single color.  
  Contains all primary colors (P50-900) and alt colors (A100-700). Also recommends an good set of alt colors to go with your alt color. Also recommends a lighter and dark variant of P500. 

<dl>
  <dt>Swatches</dt>
  <dd>Google's color swatches, can use the pregenerated ones, generate it from a single color, or from an image.  
  
  Colors:
  <ul>
    <li>Primary and alt color sets based on a primary color (P50-P900 & A100 - A700)
    <ul><li>Primary color can be adjusted to ensure no duplicates or to ensure a color spread similar to Google's</li></ul></li>
    <li>Light variant and Dark variant based on primary color</li>
    <li>Suggested alt color set made to go well with your primary color set (C100 - C700)
        <ul><li>If generating from most common color in an image alt can be based on the image instead of primary color</li></ul></li>
  </ul>  
  </dd>

  <dt>Palettes</dt>
  <dd>Android's color palette system. Only can be generated from an image. 
  
  Colors:
  <ul>
    <li> Vibrant, Vibrant Dark, Vibrant Light, Muted, Muted Dark, and Muted Light
    <ul><li>Will be able to fallback on the palette system or your own choice if a color is not generated</li></ul></li>
  </ul>
  </dd>
  
  
  <dt>Other goodies</dt>
  <dd>In order of likelyhood 
  <ul>
    <li>Safe text color - Will tell you what color to make your text that is on another color</li>
    <li>A very fast image blur possibly with image cropping</li>
    <li>Background wallpaper fetching</li>
    <li>Image from URL downloading?</li>
    <li>Image extraction from icon if that does not make it into TaskbarPlugin</li>
    <li>Ability to set windows theme color if that does not make it into TaskbarPlugin</li>
  </ul>  
  </dd>
</dl>

### Tentative Roadmap:
**For v1.0**  
[x] Import the 13 Material design color palettes  
[x] Implement autogeneration of a swatch based on any color  
[x] Auto complimentary alt color picking  
[ ] Light dark variants  
[ ] Swatch color safety features  
[ ] Implement safe text color generation  

**For v1.5**   
[ ] Implment color from image with average and most common color extraction which can then be used for swatch generation  
[ ] Add ability to crop image  

**For v2.0**  
[ ] Implement color palette from image  
[ ] Palette safety features  

**For v2.5**  
[ ] Get desktop wallpaper  
[ ] Extract icon color from exe  

**For v3.0**
Add functionallity similar to color thief to get N most commmon colors in image in no particular order and let app handle it
Nested variables?
