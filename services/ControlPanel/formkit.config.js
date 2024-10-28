import { el } from '@formkit/i18n';
import { defineFormKitConfig } from '@formkit/vue';
import '@formkit/themes/genesis';
import { genesisIcons } from '@formkit/icons';


const legends = ['checkbox_multi', 'radio_multi', 'repeater', 'transferlist'];

function addAsteriskPlugin(node) {
    if (['button', 'submit', 'hidden', 'group', 'list', 'meta'].includes(node.props.type)) return;

    node.on('created', () => {
        const legendOrLabel = legends.includes(`${node.props.type}${node.props.options ? '_multi' : ''}`) ? 'legend' : 'label';

        node.props.definition.schemaMemoKey = `required_${node.props.definition.schemaMemoKey}`

        node.context.isRequired = node.props.parsedRules.some(rule => rule.name === 'required')

        node.on('prop:validation', () => {
            node.context.isRequired = node.props.parsedRules.some(rule => rule.name === 'required')
        })

        const schemaFn = node.props.definition.schema
        node.props.definition.schema = (sectionsSchema = {}) => {
            sectionsSchema[legendOrLabel] = {
                children: ['$label', {
                    $el: 'span',
                    if: '$isRequired',
                    attrs: {
                        class: '$classes.asterisk',
                    },
                    children: ['*']
                }]
            }

            return schemaFn(sectionsSchema)
        }
    })
}

export default defineFormKitConfig({
    plugins: [addAsteriskPlugin],
    locales: { el },
    locale: 'el',
    icons: {
        ...genesisIcons
    }
})
