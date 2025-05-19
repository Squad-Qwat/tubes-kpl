import Footer from "../../../components/common/footer/footer"
import Navbar from "../../../components/common/navbar/navbar"
import HeroSection from "./components/hero-section"
import StatsSection from "./components/stats-section"

function Home() {
  return (
    <>
      <Navbar/>
      <HeroSection/>
      <StatsSection/>
      <Footer/>
    </>
  )
}

export default Home
