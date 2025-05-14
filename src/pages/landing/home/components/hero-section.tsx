import { Link } from 'react-router'
import { Button } from '../../../../components/ui/button'
import { ArrowRight, Check, Mail, SendHorizonal } from 'lucide-react'
import { AuroraText } from '../../../../components/magicui/aurora-text'

function HeroSection() {
  return (
    <section className="overflow-hidden">
      <div className="relative mt-17 py-8 md:py-16 border-b z-10 bg-linear-to-tr from-slate-200 to-white">
        <div className="mx-auto max-w-6xl px-6">
          <div className="sm:mx-auto lg:mr-auto lg:mt-0 space-y-4 lg:space-y-8">
            <Link
              to="/"
              className="relative z-20 hover:border-primary rounded-(--radius) flex w-fit items-center gap-2 border p-1 pr-3 bg-card"
            >
              <span className="bg-accent rounded-[calc(var(--radius)-0.25rem)] px-2 py-1 text-xs">
                New
              </span>
              <span className="text-sm">Introduction PaperNest!</span>
              <span className="bg-border block h-4 w-px"></span>

              <ArrowRight className="size-4" />
            </Link>

            <div className="space-y-4">
              <h1 className="max-w-2xl text-balance text-5xl font-mono font-medium uppercase md:text-6xl">
                Write document easier with{' '}
                <AuroraText
                  className="font-sans font-black uppercase"
                  colors={[
                    'oklch(85.2% 0.199 91.936)',
                    'oklch(59.1% 0.293 322.896)',
                    'oklch(59.1% 0.293 322.896)',
                  ]}
                >
                  PaperNest!
                </AuroraText>
              </h1>

              <p className="max-w-2xl text-pretty text-lg">
                Real-time collaboration for all documentation in Markdown
              </p>
            </div>

            <div className="flex items-center gap-2">
              <form action="" className="max-w-sm">
                <div className="bg-background has-[input:focus]:ring-primary relative grid grid-cols-[1fr_auto] items-center rounded-[calc(var(--radius)+0.125rem)] border pr-2 shadow shadow-zinc-950/5 has-[input:focus]:ring-1 hover:border-primary">
                  <Mail className="pointer-events-none absolute inset-y-0 left-4 my-auto size-4" />

                  <input
                    placeholder="Your mail address"
                    className="h-12 w-full bg-transparent pl-12 pr-4 focus:outline-none"
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
              <ul className="list-inside space-y-1 text-xs sm:text-base flex flex-col sm:flex-row gap-x-4">
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
          </div>
        </div>
      </div>
    </section>
  )
}

export default HeroSection
