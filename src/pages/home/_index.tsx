import Footer from "../../components/common/footer"
import Navbar from "../../components/common/navbar"
import ContentSection from "./components/content-section"
import HeroSection from "./components/hero-section"
import StatsSection from "./components/stats-section"

function HomePage() {
  return (
    <>
      <Navbar/>
      <HeroSection/>
      <StatsSection/>
      <ContentSection/>
      <Footer/>
    </>
  )
}

export default HomePage
