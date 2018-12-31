// automatically import all files ending in *.stories.tsx
import { configure } from '@storybook/react';
const req = require.context('../src', true, /.stories.tsx$/);

function loadStories() {
  req.keys().forEach(req);
}

configure(loadStories, module);