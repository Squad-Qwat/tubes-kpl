import bcrypt from 'bcrypt';

const saltRounds = 10;

const hashPassword = async (plainPassword: string): Promise<string> => {
  const hashedPassword = await bcrypt.hash(plainPassword, saltRounds);
  return hashedPassword;
};

const isPasswordValid = async (plainPassword: string, hashedPassword: string): Promise<boolean> => {
  const match = await bcrypt.compare(plainPassword, hashedPassword);
  return match;
};

export { hashPassword, isPasswordValid };
