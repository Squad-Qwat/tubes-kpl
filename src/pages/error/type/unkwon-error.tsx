import { Link } from 'react-router';

export default function UnknownError() {
  return (
    <div>
      <h1>Terjadi Kesalahan</h1>
      <p>Maaf, terjadi kesalahan yang tidak diketahui.</p>
      <p>
        <Link to="/">Kembali ke Beranda</Link>
      </p>
    </div>
  );
}
