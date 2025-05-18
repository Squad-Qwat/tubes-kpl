function generateGuid() :string {
  const buf = new Uint8Array(16);
  crypto.getRandomValues(buf);

  buf[6] = (buf[6] & 0x0f) | 0x40;
  buf[8] = (buf[8] & 0x3f) | 0x80;

  const byteToHex = [];
  for (let i = 0; i < 256; ++i) {
    byteToHex.push((i + 0x100).toString(16).substr(1));
  }

  return (
    byteToHex[buf[0]] +
    byteToHex[buf[1]] +
    byteToHex[buf[2]] +
    byteToHex[buf[3]] +
    '-' +
    byteToHex[buf[4]] +
    byteToHex[buf[5]] +
    '-' +
    byteToHex[buf[6]] +
    byteToHex[buf[7]] +
    '-' +
    byteToHex[buf[8]] +
    byteToHex[buf[9]] +
    '-' +
    byteToHex[buf[10]] +
    byteToHex[buf[11]] +
    byteToHex[buf[12]] +
    byteToHex[buf[13]] +
    byteToHex[buf[14]] +
    byteToHex[buf[15]]
  );
}

function generateRandomNumber() :string { 
  const length = 6
  let token = ''

  const characters ='0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'
  const charactersLength = characters.length

  for (let i = 0; i < length; i++) {
    const randomIndex = Math.floor(Math.random() * charactersLength)
    token += characters.at(randomIndex)
  }

  return token
}

export { generateGuid, generateRandomNumber }