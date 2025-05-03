export default function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
      <header className="fixed top-0 w-full">
        Protected header with projects, notifications, username, logout button
      </header>
      <main>{children}</main>
    </>
  );
}
