import AppLayout from '../components/layout/AppLayout'
import NoteCanvas from '../components/notes/NoteCanvas'
import { useNotes } from '../contexts/NotesContext'

/**
 * WorkspacePage — the main application view
 * Contains the sidebar + canvas layout. New notes are created instantly.
 */
function WorkspacePage() {
  const { createNote } = useNotes()

  const handleCreateNote = async () => {
    try {
      await createNote({ title: 'Untitled', content: ' ' })
    } catch (err) {
      console.error('Failed to create note:', err)
    }
  }

  return (
    <AppLayout onCreateNote={handleCreateNote}>
      <NoteCanvas />
    </AppLayout>
  )
}

export default WorkspacePage
