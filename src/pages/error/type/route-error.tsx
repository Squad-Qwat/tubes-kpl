import { Link } from 'react-router';

interface RouteErrorProps {
  status: number;
  statusText: string;
  message?: string;
}

export default function RouteError({ status, statusText, message }: RouteErrorProps) {
  return (
    <div>
      <h1>
        {status} - {statusText}
      </h1>
      <p>{message || 'Terjadi kesalahan pada permintaan.'}</p>
      <p>
        <Link to="/">Kembali ke Beranda</Link>
      </p>
    </div>
  );
}
