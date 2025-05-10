import { Link } from 'react-router';

interface InternalErrorProps {
  message: string;
}

export default function InternalError({ message }: InternalErrorProps) {
  return (
    <div>
      <h1>500 - Kesalahan Internal</h1>
      <p>{message}</p>
      <p>
        <Link to="/">Kembali ke Beranda</Link>
      </p>
    </div>
  );
}
