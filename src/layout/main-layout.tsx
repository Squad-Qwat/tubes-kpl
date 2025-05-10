import { Outlet } from 'react-router'
import Navbar from '../components/common/navbar'
import Footer from '../components/common/footer'

function MainLayout() {
  return (
    <div>
      <Navbar />
      <main>
        <Outlet />
      </main>
      <Footer />
    </div>
  )
}

export default MainLayout
