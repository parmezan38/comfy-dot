export function ColorFilter(str: string): string {
    const index = {
        h: str.indexOf('h'),
        s: str.indexOf('s'),
        l: str.indexOf('l')
    };
    let hsl = 'hsl(';
    hsl += str.substring(index.h + 1, index.s) + ',';
    hsl += str.substring(index.s + 1, index.l) + '%,';
    hsl += str.substring(index.l + 1, str.length) + '%)';
    return hsl;
}

export function FormatName(name): string {
  const returnName = name.replace(/_/g, ' ');
  return returnName.replace(/\w\S*/g, function (str) {
      return str.charAt(0).toUpperCase() + str.substr(1).toLowerCase();
  });
}

export function DecapitalizaAndRemoveSpaces(name): string {
  const returnName = name.replace(/ /g, '_');
  return returnName.toLowerCase();
}