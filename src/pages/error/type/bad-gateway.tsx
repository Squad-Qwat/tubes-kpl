import { Link } from 'react-router';

export default function BadGateway() {
  return (
    <div>
      <h1>502 - Bad Gateway</h1>
      <p>Server menerima respon yang tidak valid dari upstream.</p>
      <p>
        <Link to="/">Kembali ke Beranda</Link>
      </p>
    </div>
  );
}
