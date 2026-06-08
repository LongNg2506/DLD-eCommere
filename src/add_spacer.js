const fs = require('fs');
const path = 'd:/LongNg(Aptec)/DLD-eCommere/src/Views/Home/Index.cshtml';
let content = fs.readFileSync(path, 'utf8');

// Add carousel-spacer after each products-grid closing div
// Pattern: </div>\n (closing products-grid) followed by \n </div> (closing carousel-track)
const pattern = /(<div class="products-grid">[\s\S]*?<\/div>)\r?\n(\s*<\/div>\r?\n\s*<button class="carousel-btn carousel-next")/g;

let count = 0;
content = content.replace(pattern, (match, gridEnd, rest) => {
  count++;
  return gridEnd + '\r\n<div class="carousel-spacer"></div>' + rest;
});

fs.writeFileSync(path, content, 'utf8');
console.log('Added carousel-spacer in', count, 'locations');
