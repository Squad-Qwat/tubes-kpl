import { Button } from '../../../../components/ui/button'
import { Check, Mail, SendHorizonal } from 'lucide-react'
import { AuroraText } from '../../../../components/magicui/aurora-text'
import GridLayout from '../../../../components/magicui/grid-layout'

function HeroSection() {
  return (
    <section className="relative overflow-hidden px-6 lg:px-0">
      <div className="mt-14 lg:mt-17">
        <div className="mx-auto max-w-7xl">
          <GridLayout
            crosshairs={{
              topLeft: true,
              bottomRight: true,
            }}
            className="px-4 md:px-8 py-8 md:py-32"
            lineVariant="center"
          >
            <div className="sm:mx-auto lg:mr-auto text-center lg:mt-0 space-y-4 lg:space-y-8">
              <div className="space-y-4">
                <h1 className="text-center text-balance title-1">
                  Write document easier with {''}
                  <AuroraText
                    className="font-sans font-black uppercase"
                    colors={[
                      'oklch(85.2% 0.199 91.936)',
                      'oklch(59.1% 0.293 322.896)',
                      'oklch(59.1% 0.293 322.896)',
                    ]} /* TODO: KATA DARREL SILAU, TUNGGU USER TANGGAPANNYA */
                  >
                    PaperNest!
                  </AuroraText>
                </h1>

                <p className="paragraph-large">
                  Real-time collaboration for all documentation in Markdown
                </p>
              </div>

              <div className="flex justify-center items-center gap-2">
                <form action="" className="max-w-sm">
                  <div className="bg-background has-[input:focus]:ring-primary relative grid grid-cols-[1fr_auto] items-center rounded-[calc(var(--radius)+0.125rem)] border pr-2 shadow shadow-zinc-950/5 has-[input:focus]:ring-1 hover:border-primary">
                    <Mail className="pointer-events-none absolute inset-y-0 left-4 my-auto size-4" />

                    <input
                      placeholder="Your mail address"
                      className="h-12 w-full bg-transparent pl-12 pr-4 focus:outline-none md:text-base text-sm"
                      type="email"
                    />

                    <div className="md:pr-1.5 lg:pr-0">
                      <Button aria-label="submit" size="sm">
                        <span className="hidden md:block">Create Account</span>
                        <SendHorizonal
                          className="relative mx-auto size-5 md:hidden"
                          strokeWidth={2}
                        />
                      </Button>
                    </div>
                  </div>
                </form>
              </div>

              <div>
                <ul className="list-inside justify-center space-y-1 text-xs sm:text-base flex flex-col items-center caption flex-wrap sm:flex-row gap-2 md:gap-4">
                  <li className="flex gap-x-2 items-center">
                    <Check className="size-4 text-green-700" />
                    Faster & Modern
                  </li>
                  <li className="flex gap-x-2 items-center">
                    <Check className="size-4 text-green-700" />
                    100% Safe
                  </li>
                  <li className="flex gap-x-2 items-center">
                    <Check className="size-4 text-green-700" />
                    Real-time Collaboration
                  </li>
                  <li className="flex gap-x-2 items-center">
                    <Check className="size-4 text-green-700" />
                    Totally Free
                  </li>
                </ul>
              </div>

              <img
                className="rounded-(--radius) grayscale"
                src="https://images.unsplash.com/photo-1530099486328-e021101a494a?q=80&w=2747&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                alt="team image"
                height=""
                width=""
                loading="lazy"
              />
            </div>
          </GridLayout>
        </div>
      </div>
    </section>
  )
}

export default HeroSection
