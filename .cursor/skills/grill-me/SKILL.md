---
name: grill-me
description: >-
  Relentless Russian-language interview to stress-test a plan or design.
  Use only when the user explicitly runs /grill-me or asks to grill the plan.
disable-model-invocation: true
---

# grill-me

Жёсткое интервью по плану/дизайну (по мотивам [mattpocock grilling](https://github.com/mattpocock/skills/tree/main/skills/productivity)).

## Когда

Только по явному запросу: `/grill-me`, `grill-me`, «прогони grill», «погрилль план».

Не запускать автоматически в Plan mode и не перед каждым CreatePlan.

## Как вести

1. Все вопросы — на русском.
2. По одному вопросу за раз; ждать ответ перед следующим.
3. К каждому вопросу — краткая своя рекомендация ответа.
4. Идти по дереву решений; зависимости разрешать по очереди.
5. Факты из репо/файлов — смотреть инструментами; у пользователя спрашивать только решения.
6. Не писать код и не менять файлы, пока пользователь не подтвердит shared understanding.

## Цель

Дойти до общего понимания дизайна без дыр в дереве решений — затем остановиться и дать решить, реализовывать ли.
